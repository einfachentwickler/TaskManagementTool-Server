using Application.Commands.Auth.Login.Models;
using Application.Services.IdentityUserManagement;
using Application.Services.Jwt.AccessToken;
using Application.Services.Jwt.RefreshToken;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Auth.Login;

public class UserLoginHandler(
    IIdentityUserManagerWrapper userManager,
    IValidator<UserLoginCommand> requestValidator,
    IJwtRefreshTokenGenerator jwtRefreshTokenGenerator,
    IJwtAccessTokenBuilder jwtSecurityTokenBuilder,
    ITaskManagementToolDbContext taskManagementToolDbContext,
    IOptions<AuthOptions> options,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<UserLoginCommand, UserLoginResponse>
{
    private readonly IIdentityUserManagerWrapper _userManager = userManager;
    private readonly IValidator<UserLoginCommand> _requestValidator = requestValidator;
    private readonly IJwtAccessTokenBuilder _jwtSecurityTokenBuilder = jwtSecurityTokenBuilder;
    private readonly IJwtRefreshTokenGenerator _jwtRefreshTokenGenerator = jwtRefreshTokenGenerator;
    private readonly ITaskManagementToolDbContext _dbContext = taskManagementToolDbContext;
    private readonly AuthOptions _authOptions = options.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<UserLoginResponse> Handle(UserLoginCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors[0];
            throw new CustomException<UserLoginErrorCode>(Enum.Parse<UserLoginErrorCode>(firstError.ErrorCode), firstError.ErrorMessage);
        }

        var user = await _userManager.FindByEmailAsync(command.Email);
        var passwordValid = user != null && await _userManager.CheckPasswordAsync(user, command.Password);

        if (user == null || !passwordValid)
        {
            throw new CustomException<UserLoginErrorCode>(UserLoginErrorCode.InvalidCredentials, UserLoginErrorMessages.InvalidCredentials);
        }

        if (user.IsBlocked)
        {
            throw new CustomException<UserLoginErrorCode>(UserLoginErrorCode.BlockedEmail, UserLoginErrorMessages.BlockedEmail);
        }

        var buildTokenResponse = _jwtSecurityTokenBuilder.Build(user.Id, user.Role, command.Email);

        var refreshToken = _jwtRefreshTokenGenerator.Generate();

        await _dbContext.RefreshTokens.AddAsync(new RefreshTokenEntity
        {
            UserEmail = user.Email,
            TokenHash = _jwtRefreshTokenGenerator.Hash(refreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_authOptions.RefreshTokenLifetimeDays),
            UserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString(),
            CreatedByIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString(),
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserLoginResponse
        {
            AccessToken = buildTokenResponse.Token,
            Expires = buildTokenResponse.Expires,
            RefreshToken = refreshToken
        };
    }
}
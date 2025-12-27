using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.RefreshToken.Models;
using Application.Services.IdentityUserManagement;
using Application.Services.Jwt.AccessToken;
using Application.Services.Jwt.RefreshToken;
using Infrastructure.Context;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Exceptions;
using System;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Auth.RefreshToken;

public class RefreshTokenHandler(
    ITaskManagementToolDbContext dbContext,
    IIdentityUserManagerWrapper userManager,
    IJwtAccessTokenBuilder jwtBuilder,
    IJwtRefreshTokenGenerator jwtRefreshTokenGenerator,
    IOptions<AuthOptions> options,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<RefreshTokenCommand, UserLoginResponse>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IIdentityUserManagerWrapper _userManager = userManager;
    private readonly IJwtAccessTokenBuilder _jwtBuilder = jwtBuilder;
    private readonly IJwtRefreshTokenGenerator _jwtRefreshTokenGenerator = jwtRefreshTokenGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly AuthOptions _options = options.Value;

    public async Task<UserLoginResponse> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var tokenHash = _jwtRefreshTokenGenerator.Hash(command.RefreshToken);

        var storedToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

        if (storedToken == null)
            throw new CustomException<RefreshTokenErrorCode>(RefreshTokenErrorCode.InvalidToken, RefreshTokenErrorMessages.InvalidToken);

        if (storedToken.RevokedAt != null)
        {
            var userTokens = _dbContext.RefreshTokens.Where(t => t.UserEmail == storedToken.UserEmail && t.RevokedAt == null);

            foreach (var token in userTokens)
                token.RevokedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            throw new SecurityException($"Refresh token reuse detected, email - {storedToken.UserEmail}");
        }

        var user = await _userManager.FindByEmailAsync(storedToken.UserEmail);

        storedToken.RevokedAt = DateTime.UtcNow;

        var newRefreshToken = _jwtRefreshTokenGenerator.Generate();

        await _dbContext.RefreshTokens.AddAsync(new RefreshTokenEntity
        {
            UserEmail = user.Email,
            TokenHash = _jwtRefreshTokenGenerator.Hash(newRefreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenLifetimeDays),
            UserAgent = _httpContextAccessor.HttpContext.Request.Headers.UserAgent,
            CreatedByIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString(),
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var accessToken = _jwtBuilder.Build(user.Id, user.Role, user.Email);

        return new UserLoginResponse
        {
            AccessToken = accessToken.Token,
            Expires = accessToken.Expires,
            RefreshToken = newRefreshToken
        };
    }
}
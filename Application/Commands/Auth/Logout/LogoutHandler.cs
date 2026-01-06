using Application.Commands.Auth.Logout.Models;
using Application.Services.Abstractions.DateTimeGeneration;
using Application.Services.Jwt.RefreshToken;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Auth.Logout;

public class LogoutHandler(
    ITaskManagementToolDbContext dbContext,
    IJwtRefreshTokenGenerator jwtRefreshTokenGenerator,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<LogoutCommand>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IJwtRefreshTokenGenerator _jwtRefreshTokenGenerator = jwtRefreshTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var tokenHash = _jwtRefreshTokenGenerator.Hash(command.RefreshToken);

        var token = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == command.RefreshToken, cancellationToken);

        if (token != null)
        {
            token.RevokedAt = _dateTimeProvider.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
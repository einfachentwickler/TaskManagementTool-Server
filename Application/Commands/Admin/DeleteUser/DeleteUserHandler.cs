using Application.Commands.Admin.DeleteUser.Models;
using Application.Services.IdentityUserManagement;
using FluentValidation;
using Infrastructure.Context;
using LoggerService;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Admin.DeleteUser;

public class DeleteUserHandler(
    IIdentityUserManagerWrapper userManager,
    ITaskManagementToolDbContext dbContext,
    IValidator<DeleteUserCommand> validator,
    ILoggerManager loggerManager) : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IIdentityUserManagerWrapper _userManager = userManager;
    private readonly IValidator<DeleteUserCommand> _validator = validator;
    private readonly ILoggerManager _loggerManager = loggerManager;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var firstError = validationResult.Errors.First();
            throw new CustomException<DeleteUserErrorCode>(DeleteUserErrorCode.InvalidEmail, firstError.ErrorMessage);
        }

        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new CustomException<DeleteUserErrorCode>(DeleteUserErrorCode.UserNotFound, DeleteUserErrorMessages.UserNotFound);

        await _dbContext.Todos.Where(todo => todo.Creator.Email == request.Email).ExecuteDeleteAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var identityResult = await _userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            _loggerManager.LogWarn(string.Join(", ", identityResult.Errors.Select(x => x.Code + " " + x.Description)));
            throw new CustomException<DeleteUserErrorCode>(DeleteUserErrorCode.InternalServerError, DeleteUserErrorMessages.InternalServerError);
        }

        return new Unit();
    }
}
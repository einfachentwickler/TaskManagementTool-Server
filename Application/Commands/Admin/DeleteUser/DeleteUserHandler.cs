using Application.Commands.Admin.DeleteUser.Models;
using Application.Services.IdentityUserManagement;
using FluentValidation;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.Common.Exceptions;

namespace Application.Commands.Admin.DeleteUser;

public class DeleteUserHandler(IIdentityUserManagerWrapper userManager, ITaskManagementToolDbContext dbContext, IValidator<DeleteUserCommand> validator) : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly ITaskManagementToolDbContext _dbContext = dbContext;
    private readonly IIdentityUserManagerWrapper _userManager = userManager;
    private readonly IValidator<DeleteUserCommand> _validator = validator;

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
            throw new CustomException<DeleteUserErrorCode>(DeleteUserErrorCode.InternalServerError, DeleteUserErrorMessages.InternalServerError);
            //todo log errors
        }

        return new Unit();
    }
}
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Dto.AuthModels;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;

namespace TaskManagementTool.BusinessLogic.Interfaces;

public interface IAuthHandler
{
    public Task<UserManagerResponse> RegisterUserAsync(RegisterDto model);

    public Task<UserManagerResponse> LoginUserAsync(LoginDto model);

    public Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordDto model);
}
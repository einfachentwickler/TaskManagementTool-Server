using System.ComponentModel.DataAnnotations;

namespace TaskManagementTool.BusinessLogic.Dto.AuthModels;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Email is a required field")]
    [EmailAddress(ErrorMessage = "Ivalid email address")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Password is a required field")]
    [MinLength(1, ErrorMessage = "Min length is 1")]
    public string CurrentPassword { get; init; }

    [Required(ErrorMessage = "New password is a required field")]
    [MinLength(1, ErrorMessage = "Min length is 1")]
    public string NewPassword { get; init; }

    [Required(ErrorMessage = "New password confirmation is required")]
    [MinLength(1, ErrorMessage = "Min length is 1")]
    public string ConfirmNewPassword { get; init; }
}
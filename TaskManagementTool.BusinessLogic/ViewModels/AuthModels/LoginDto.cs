using System.ComponentModel.DataAnnotations;

namespace TaskManagementTool.BusinessLogic.ViewModels.AuthModels
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is a required field")]
        [EmailAddress(ErrorMessage = "Ivalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        public string Password { get; set; }
    }
}

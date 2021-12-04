using System.ComponentModel.DataAnnotations;

namespace TaskManagementTool.BusinessLogic.ViewModels.AuthModels
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is a required field")]
        [EmailAddress(ErrorMessage = "Ivalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is a required field")]
        [Range(0, 130, ErrorMessage = "Age must be beetween 0 and 130")]
        public int Age { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TaskManagementTool.BusinessLogic.ViewModels.ToDoModels
{
    public class UpdateTodoDto
    {
        [Required(ErrorMessage = "Id is a required field")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Content is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(400, ErrorMessage = "Max length is 400")]
        public string Content { get; set; }

        [Required(ErrorMessage = "IsCompleted is a required field")]
        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Importance is a required field")]
        [Range(1, 10, ErrorMessage = "Importance must be beetween 1 and 10")]
        public int Importance { get; set; }
    }
}

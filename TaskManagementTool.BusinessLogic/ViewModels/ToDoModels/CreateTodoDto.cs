using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementTool.BusinessLogic.ViewModels.ToDoModels
{
    public class CreateTodoDto
    {
        [Required(ErrorMessage = "Name is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Content is a required field")]
        [MinLength(1, ErrorMessage = "Min length is 1")]
        [MaxLength(500, ErrorMessage = "Max length is 500")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Importance is a required field")]
        [Range(1, 10, ErrorMessage = "Importance must be beetween 1 and 10")]
        public int Importance { get; set; }

        [JsonIgnore]
        public string CreatorId { get; set; }
    }
}

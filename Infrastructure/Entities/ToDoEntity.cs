using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class ToDoEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Content { get; set; }

    public bool IsCompleted { get; set; }

    public int Importance { get; set; }

    public UserEntity Creator { get; set; }

    [ForeignKey("User")]
    public string CreatorId { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class ToDoEntity
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Content { get; set; }

    public required bool IsCompleted { get; set; }

    public required int Importance { get; set; }

    public UserEntity Creator { get; set; }

    [ForeignKey("User")]
    public required string CreatorId { get; set; }
}
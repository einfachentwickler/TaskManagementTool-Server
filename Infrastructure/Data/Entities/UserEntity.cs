using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities;

public class UserEntity : IdentityUser
{
    [Key]
    public override string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int Age { get; set; }

    public bool IsBlocked { get; set; }

    public string Role { get; set; }
}
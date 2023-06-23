using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("users")]
[Index(nameof(Id), IsUnique = true)]
public class User
{
    [Key] public int Id { get; init; }
    public string FIO { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime LastLogin { get; set; }
}
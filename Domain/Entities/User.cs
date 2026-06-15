using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Users")]
public class User
{
    [Key]
    public Guid UserId { get; set; } =  Guid.NewGuid();

    
    public string Login { get; set; } =  string.Empty;
    
    public string PasswordHash { get; set; } =  string.Empty;

    public Role Role { get; set; }
}
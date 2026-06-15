using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Token
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    
    public virtual User User { get; set; } = null!;
}
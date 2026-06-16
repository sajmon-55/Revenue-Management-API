namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public string Login { get; set; } =  string.Empty;
    public string PasswordHash { get; set; } =  string.Empty;
    public Role Role { get; set; }

    public virtual Token Token { get; set; } = null!;
}
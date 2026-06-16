namespace Domain.Entities.Clients;

public class IndividualClient
{
    public int ClientId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Pesel { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    
    public virtual Client Client { get; set; } = null!;
}
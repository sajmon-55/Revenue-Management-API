namespace Domain.Entities.Clients;

public class IndividualClient : Client
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Pesel { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
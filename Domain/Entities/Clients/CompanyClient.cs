namespace Domain.Entities.Clients;

public class CompanyClient
{
    public int ClientId { get; set; }
    public string Name { get; set; }  = string.Empty;
    public string Krs { get; set; } =  string.Empty;
    
    public virtual Client Client { get; set; } = null!;
}
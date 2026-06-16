namespace Domain.Entities.Softwares;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public ICollection<SoftwareDiscount> SoftwareDiscounts { get; set; } = new List<SoftwareDiscount>();
}
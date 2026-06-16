using Domain.Entities.Contracts;
using Domain.Entities.Subscriptions;

namespace Domain.Entities.Softwares;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public ICollection<SoftwareDiscount> SoftwareDiscounts { get; set; } = new List<SoftwareDiscount>();
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public ICollection<Subscription>  Subscriptions { get; set; } = new List<Subscription>(); 
}
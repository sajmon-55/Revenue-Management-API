using Domain.Entities.Clients;
using Domain.Entities.Softwares;

namespace Domain.Entities.Subscriptions;

public class Subscription
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RenewalPeriod { get; set; }
    public decimal PricePerPeriod { get; set; }

    public virtual Client Client { get; set; } = null!;
    public virtual Software Software { get; set; } = null!;
    public virtual SubscriptionStatusType SubscriptionStatusType { get; set; } = null!;
    
    public virtual ICollection<SubscriptionPayment> SubscriptionPayments { get; set; } =
        new List<SubscriptionPayment>();
}
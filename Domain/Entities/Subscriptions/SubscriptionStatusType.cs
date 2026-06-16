namespace Domain.Entities.Subscriptions;

public class SubscriptionStatusType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
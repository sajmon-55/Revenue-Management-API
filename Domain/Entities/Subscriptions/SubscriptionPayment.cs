namespace Domain.Entities.Subscriptions;

public class SubscriptionPayment
{
    public int Id { get; set; }
    public int SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public DateOnly PaymentDate { get; set; }
    public DateOnly PeriodStart { get; set; }
    public  DateOnly PeriodEnd { get; set; }

    public virtual Subscription Subscription { get; set; } = null!;
}
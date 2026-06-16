namespace Domain.Entities.Softwares;

public class SoftwareDiscount
{
    public int SoftwareId { get; set; }
    public virtual Software Software { get; set; } = null!;
    public int DiscountId { get; set; }
    public virtual Discount Discount { get; set; } = null!;
}
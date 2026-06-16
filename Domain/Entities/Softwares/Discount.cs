namespace Domain.Entities.Softwares;

public class Discount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }

    public virtual ICollection<SoftwareDiscount> SoftwareDiscounts { get; set; } = new List<SoftwareDiscount>();
}
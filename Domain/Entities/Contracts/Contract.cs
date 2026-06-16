using Domain.Entities.Clients;
using Domain.Entities.Softwares;
namespace Domain.Entities.Contracts;


public class Contract
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public string SoftwareVersion { get; set; } =  string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Price { get; set; }
    public int YearsOfSupport { get; set; }
    public int StatusId { get; set; }
    
    public virtual Client Client { get; set; } = null!;
    public virtual Software Software { get; set; } = null!;
    public virtual ContractStatusType ContractStatusType { get; set; } = null!;

    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = [];
}
namespace Domain.Entities;

public class ContractStatusType
{
    public int Id { get; set; }
    public string Name { get; set; } =  string.Empty;

    public virtual ICollection<Contract> Contracts { get; set; } = [];
}
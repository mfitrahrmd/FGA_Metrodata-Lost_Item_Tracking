using Domain.Common;

namespace Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    
    public Guid EmployeeId { get; set; }
    
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<ItemActions>? ItemActions { get; set; }
}
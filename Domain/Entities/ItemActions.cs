#region

using Domain.Common;

#endregion

namespace Domain.Entities;

public class ItemActions : BaseEntity
{
    public DateTime Time { get; set; }
    public bool IsApproved { get; set; } = false;
    
    public Guid ItemId { get; set; }
    public Guid ActionId { get; set; }
    public Guid EmployeeId { get; set; }
    
    public virtual Item? Item { get; set; }
    public virtual Action? Action { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual Status? Status { get; set; }
}
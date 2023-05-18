#region

using Domain.Common;

#endregion

namespace Domain.Entities;

public class ItemActions : BaseEntity
{
    public DateTime Time { get; set; }
    
    public Guid ItemId { get; set; }
    public Guid ActionId { get; set; }
    
    public virtual Item? Item { get; set; }
    public virtual Action? Action { get; set; }
}
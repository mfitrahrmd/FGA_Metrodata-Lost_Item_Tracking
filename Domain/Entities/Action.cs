#region

using Domain.Common;

#endregion

namespace Domain.Entities;

public class Action : BaseEntity
{
    public string Name { get; set; }
    
    public virtual ICollection<ItemActions>? ItemActions { get; set; }
}
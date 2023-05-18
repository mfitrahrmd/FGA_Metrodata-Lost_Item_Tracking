#region

using Domain.Common;

#endregion

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    
    public virtual ICollection<AccountRoles>? AccountRoles { get; set; }
}
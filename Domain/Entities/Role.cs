using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    
    public virtual ICollection<AccountRoles>? AccountRoles { get; set; }
}
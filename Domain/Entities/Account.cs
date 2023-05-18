#region

using Domain.Common;

#endregion

namespace Domain.Entities;

public class Account : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public string Password { get; set; }
    
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<AccountRoles>? AccountRoles { get; set; }
}
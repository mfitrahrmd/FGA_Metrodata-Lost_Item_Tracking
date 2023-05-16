using Domain.Common;

namespace Domain.Entities;

public class AccountRoles : BaseEntity
{
    public Guid AccountId { get; set; }
    public Guid RoleId { get; set; }

    public virtual Account? Account { get; set; }
    public virtual Role? Role { get; set; }
}
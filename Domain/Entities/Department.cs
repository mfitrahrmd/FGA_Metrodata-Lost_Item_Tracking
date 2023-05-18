#region

using Domain.Common;

#endregion

namespace Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; }
    
    public virtual ICollection<Employee>? Employees { get; set; }
}
#region

using Domain.Common;
using Domain.Enums;

#endregion

namespace Domain.Entities;

public class Employee : BaseEntity
{
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid DepartmentId { get; set; }
    
    public virtual Department? Department { get; set; }
    public virtual Account Account { get; set; }
    public virtual ICollection<ItemActions>? ItemActions { get; set; }
}
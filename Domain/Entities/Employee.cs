using Domain.Common;
using Domain.Enums;

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
    
    public virtual ICollection<Item>? Items { get; set; }
}
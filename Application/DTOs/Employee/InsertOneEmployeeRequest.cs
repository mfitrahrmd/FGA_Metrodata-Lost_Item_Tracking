#region

using System.ComponentModel.DataAnnotations;
using Domain.Enums;

#endregion

namespace Application.DTOs.Employee;

public class InsertOneEmployeeRequest
{
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
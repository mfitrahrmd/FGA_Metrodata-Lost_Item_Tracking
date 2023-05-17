using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Employee;

public class EmployeeDTO
{
    public Guid Id { get; set; }
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
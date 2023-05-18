#region

using Domain.Enums;

#endregion

namespace Application.DTOs.Account;

public class RegisterRequest
{
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid DepartmentId { get; set; }
    public string Password { get; set; }
}
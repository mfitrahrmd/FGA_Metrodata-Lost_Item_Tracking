namespace Application.DTOs.Account;

public class InsertOneAccountRequest
{
    public Guid EmployeeId { get; set; }
    public string Password { get; set; }
}
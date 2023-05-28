namespace Application.DTOs.Account;

public class InsertAccountRequest
{
    public Guid EmployeeId { get; set; }
    public string Password { get; set; }
}
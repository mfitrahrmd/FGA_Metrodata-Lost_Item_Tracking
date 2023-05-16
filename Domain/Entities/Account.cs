namespace Domain.Entities;

public class Account
{
    public Guid EmployeeId { get; set; }
    public string Password { get; set; }
    
    public virtual Employee? Employee { get; set; }
}
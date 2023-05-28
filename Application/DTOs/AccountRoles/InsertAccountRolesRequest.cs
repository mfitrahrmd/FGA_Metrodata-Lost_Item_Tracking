namespace Application.DTOs.AccountRoles;

public class InsertAccountRolesRequest
{
    public Guid AccountId { get; set; }
    public Guid RoleId { get; set; }
}
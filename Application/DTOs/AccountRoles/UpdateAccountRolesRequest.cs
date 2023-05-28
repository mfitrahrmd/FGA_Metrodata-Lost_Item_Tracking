namespace Application.DTOs.AccountRoles;

public class UpdateAccountRolesRequest
{
    public Guid AccountId { get; set; }
    public Guid RoleId { get; set; }
}
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
public class AccountRolesController : BaseController<AccountRoles, IAccountRolesRepository, Application.DTOs.AccountRoles.InsertAccountRolesRequest, Application.DTOs.AccountRoles.UpdateAccountRolesRequest, Application.DTOs.AccountRoles.AccountRolesDTO>
{
    public AccountRolesController(IAccountRolesRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
public class RolesController : BaseController<Role, IRoleRepository, Application.DTOs.Role.InsertRoleRequest, Application.DTOs.Role.UpdateRoleRequest, Application.DTOs.Role.RoleDTO>
{
    public RolesController(IRoleRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
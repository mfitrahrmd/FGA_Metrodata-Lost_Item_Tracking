#region

using Application.DTOs.Employee;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace RESTAPI.Controllers;

[ApiController]
public class EmployeesController : BaseController<Employee, IEmployeeRepository, EmployeeDTO, InsertOneEmployeeRequest>
{
    public EmployeesController(IEmployeeRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
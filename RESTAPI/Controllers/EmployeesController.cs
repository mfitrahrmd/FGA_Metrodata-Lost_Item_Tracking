using Application.DTOs.Employee;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : BaseController<Employee, IEmployeeRepository, EmployeeDTO, InsertOneEmployeeRequest>
{
    public EmployeesController(IEmployeeRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
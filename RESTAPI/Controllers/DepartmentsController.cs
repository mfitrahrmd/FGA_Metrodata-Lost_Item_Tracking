using Application.DTOs.Department;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
public class DepartmentsController : BaseController<Department, IDepartmentRepository, Application.DTOs.Department.InsertDepartmentRequest, Application.DTOs.Department.UpdateDepartmentRequest, DepartmentDTO>
{
    public DepartmentsController(IDepartmentRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
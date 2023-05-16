using System.Net;
using Application.DTOs.Employee;
using Application.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IMapper mapper, IEmployeeRepository employeeRepository)
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<ICollection<EmployeeDTO>>>> FindAll()
    {
        SuccessResponse<ICollection<EmployeeDTO>> response;

        try
        {
            var employees = await _employeeRepository.FindAllAsync();

            response = new SuccessResponse<ICollection<EmployeeDTO>>(null,
                _mapper.Map<ICollection<EmployeeDTO>>(employees));
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new FailResponse<object>(e.Message));
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<EmployeeDTO>>> InsertOne([FromBody] InsertOneRequest request)
    {
        SuccessResponse<EmployeeDTO> response;

        try
        {
            await _employeeRepository.InsertOneAsync(_mapper.Map<Employee>(request));

            response = new SuccessResponse<EmployeeDTO>(null,
                _mapper.Map<EmployeeDTO>(_mapper.Map<Employee>(request)));
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new FailResponse<object>(e.Message));
        }

        return Ok(response);
    }
}
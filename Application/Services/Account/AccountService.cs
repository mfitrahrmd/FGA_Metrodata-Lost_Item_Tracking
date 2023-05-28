#region

using System.Security.Claims;
using Application.DTOs.Account;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Identity.Services;

#endregion

namespace Application.Services.Account;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;
    public const string RegisterDefaultRole = "User";

    public AccountService(IAccountRepository accountRepository, IRoleRepository roleRepository, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, TokenService tokenService, IMapper mapper)
    {
        (_accountRepository, _roleRepository, _employeeRepository, _departmentRepository, _tokenService, _mapper) = (accountRepository, roleRepository, employeeRepository, departmentRepository, tokenService, mapper);
    }
    
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (!await _employeeRepository.IsEmployeeUnique(_mapper.Map<Employee>(request)))
            throw new ServiceException(ErrorType.InvalidInput, "Input employee is not unique.");

        if (!await _departmentRepository.IsExistAsync(request.DepartmentId))
        {
            throw new ServiceException(ErrorType.ResourceNotFound, "Department with given Id was not found.");
        }
        
        var newAccount = new Domain.Entities.Account
        {
            Password = request.Password,
            Employee = new Employee
            {
                Nik = request.Nik,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DepartmentId = request.DepartmentId
            },
            AccountRoles = new List<AccountRoles>(new[]
            {
                new AccountRoles
                {
                    Role = await _roleRepository.FindOrCreateRoleAsync(RegisterDefaultRole)
                }
            })
        };
        
        await _accountRepository.InsertOneAsync(newAccount);

        return new RegisterResponse
        {
            Nik = newAccount.Employee.Nik
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var foundEmployee = await _employeeRepository.FindOneByNikIncludeAccount(request.Nik);

        if (foundEmployee is null)
            throw new ServiceException(ErrorType.ResourceNotFound, "Account with given Nik was not found.");

        if (!foundEmployee.Account.Password.Equals(request.Password))
            throw new ServiceException(ErrorType.Unauthenticated, "Invalid password.");

        var rolesOfAnAccount = await _accountRepository.GetRolesOfAnAccountByEmployeeId(foundEmployee.Account.EmployeeId);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, foundEmployee.Id.ToString()),
            new Claim(ClaimTypes.PrimarySid, foundEmployee.Nik),
            new Claim(ClaimTypes.Name, $"{foundEmployee.FirstName} {foundEmployee.LastName}")
        };
        
        claims.AddRange(rolesOfAnAccount.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var generatedAccessToken = _tokenService.GenerateAccessToken(claims);

        return new LoginResponse
        {
            AccessToken = generatedAccessToken
        };
    }
}
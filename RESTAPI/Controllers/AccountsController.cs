#region

using Application.DTOs.Account;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Account;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace RESTAPI.Controllers;

[ApiController]
public class AccountsController : BaseController<Account, IAccountRepository, Application.DTOs.Account.InsertAccountRequest, Application.DTOs.Account.UpdateAccountRequest, AccountDTO>
{
    private readonly AccountService _accountService;
    
    public AccountsController(IAccountRepository repository, AccountService accountService, IMapper mapper) : base(repository, mapper)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<SuccessResponse<LoginResponse>>> LoginAsync([FromBody] LoginRequest request)
    {
        try
        { 
            var result = await _accountService.LoginAsync(request);

            return Ok(new SuccessResponse<LoginResponse>(null, result));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<SuccessResponse<RegisterResponse>>> RegisterAsync([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _accountService.RegisterAsync(request);

            return Ok(new SuccessResponse<RegisterResponse>(null, result));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
}
using System.Security.Claims;
using Application.DTOs.Item;
using Application.DTOs.ItemActions;
using Application.Exceptions;
using Application.Services.Item;
using Application.Wrappers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ItemService _itemService;
    
    public ItemsController(IMapper mapper, ItemService itemService)
    {
        _mapper = mapper;
        _itemService = itemService;
    }
    
    [HttpPost]
    [Route("found")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> ReportFoundItem([FromForm] InsertFoundItemRequest request)
    {
        try
        {
            var id = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
            var nik = User.FindFirstValue(ClaimTypes.PrimarySid);
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        
            var result = await _itemService.ReportFoundItem(request, new UserIdentity(id, nik, roles));
        
            return Ok(new SuccessResponse<ItemDTO>(null, _mapper.Map<ItemDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPatch]
    [Route("found/{itemId}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> ApproveFoundItem([FromRoute] Guid itemId)
    {
        try
        {
            var result =  await _itemService.ApproveFoundItem(itemId);

            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
}
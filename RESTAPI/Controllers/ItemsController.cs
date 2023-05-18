using System.Security.Claims;
using Application.DTOs.Item;
using Application.DTOs.ItemActions;
using Application.Exceptions;
using Application.Services.Common;
using Application.Services.Item;
using Application.Services.ItemActions;
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
    private readonly ItemActionsService _itemActionsService;
    
    public ItemsController(IMapper mapper, ItemService itemService, ItemActionsService itemActionsService)
    {
        _mapper = mapper;
        _itemService = itemService;
        _itemActionsService = itemActionsService;
    }
    
    [HttpPost]
    [Route("found")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> ReportFoundItem([FromForm] InsertFoundItemRequest request)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);
        
            var result = await _itemService.ReportFoundItem(request, userIdentity);
        
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
            var result =  await _itemActionsService.ApproveItemAction(itemId, ActionType.Found);

            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("found/{itemId}/request-claim")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> RequestClaimItem([FromRoute] Guid itemId)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);
            
            var result = await _itemActionsService.AddItemAction(itemId, userIdentity, ActionType.RequestClaim);
            
            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
    
    [HttpPatch]
    [Route("found/{itemId}/request-claim/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> ApproveRequestClaimItem([FromRoute] Guid itemId)
    {
        try
        {
            var result =  await _itemActionsService.ApproveItemAction(itemId, ActionType.RequestClaim);

            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("found/{itemId}/claimed")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> UpdateStatusClaimedItem([FromRoute] Guid itemId)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);

            var result = await _itemActionsService.AddItemAction(itemId, userIdentity, ActionType.Claimed);
            
            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    private UserIdentity GetUserIdentityFromClaims(ClaimsPrincipal claimsPrincipal)
    {
        var id = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
        var nik = User.FindFirstValue(ClaimTypes.PrimarySid);
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return new UserIdentity(id, nik, roles);
    }
}
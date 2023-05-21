using System.Security.Claims;
using Application.DAOs.Item;
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
    [Route("request-found")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> RequestFoundItem([FromForm] InsertFoundItemRequest request)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);

            var result = await _itemService.RequestFoundItem(request, userIdentity.Id);

            return Ok(new SuccessResponse<ItemDTO>(null, _mapper.Map<ItemDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPatch]
    [Route("request-found/{requestId}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> ApproveRequestFoundItem([FromRoute] Guid requestId, [FromBody] ApproveFoundItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.ApproveItemAction(requestId, request.Message);

            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("found/{itemId}/request-claim")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> RequestClaimItem([FromRoute] Guid itemId)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);

            var result = await _itemActionsService.AddItemAction(itemId, userIdentity.Id, ActionType.RequestClaim);

            return Ok(new SuccessResponse<ItemDTO>(null, _mapper.Map<ItemDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPatch]
    [Route("found/request-claim/{requestId}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> ApproveRequestClaimItem([FromRoute] Guid requestId, ApproveRequestClaimItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.ApproveItemAction(requestId, request.Message);

            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("{itemId}/confirm-claimed")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> UpdateStatusClaimedItem([FromRoute] Guid itemId, [FromBody] UpdateStatusClaimedItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.AddItemAction(itemId, request.ClaimerId, ActionType.Claimed);

            return Ok(new SuccessResponse<ItemDTO>(null, _mapper.Map<ItemDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
    
    [HttpPost]
    [Route("{itemId}/confirm-found")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> UpdateStatusFoundItem([FromRoute] Guid itemId, [FromBody] UpdateStatusFoundItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.AddItemAction(itemId, request.FounderId, ActionType.Found);

            return Ok(new SuccessResponse<ItemDTO>(null, _mapper.Map<ItemDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpGet]
    [Route("found")]
    public async Task<ActionResult<SuccessResponse<ICollection<FoundItemDTO>>>> FindAllFoundItems()
    {
        try
        {
            var result = await _itemService.FindAllFoundItems();

            return Ok(new SuccessResponse<ICollection<FoundItemDTO>>(null,
                _mapper.Map<ICollection<FoundItemDTO>>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
    
    [HttpGet]
    [Route("request-found")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ICollection<RequestFoundItem>>>> FindAllRequestFoundItems([FromQuery] ActionRequestQuery query)
    {
        try
        {
            var result = await _itemService.FindAllRequestFoundItems(query);

            return Ok(new SuccessResponse<ICollection<RequestFoundItem>>(null, result));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
    
    [HttpGet]
    [Route("request-claim")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ICollection<FoundItemRequestClaim>>>> FindAllFoundItemRequestClaims([FromQuery] ActionRequestQuery query)
    {
        try
        {
            var result = await _itemService.FindAllFoundItemRequestClaims(query);

            return Ok(new SuccessResponse<ICollection<FoundItemRequestClaim>>(null, result));
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
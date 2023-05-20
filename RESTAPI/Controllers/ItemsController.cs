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
    [Route("found")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> ReportFoundItem([FromForm] InsertFoundItemRequest request)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);

            var result = await _itemService.ReportFoundItem(request, userIdentity.Id);

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
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> ApproveFoundItem([FromRoute] Guid itemId, [FromBody] ApproveFoundItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.ApproveItemAction(itemId, ActionType.Found, request.Message);

            return Ok(new SuccessResponse<ItemDTO>(null, _mapper.Map<ItemDTO>(result)));
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
    [Route("found/{itemId}/request-claim/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> ApproveRequestClaimItem([FromRoute] Guid itemId, ApproveRequestClaimItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.ApproveItemAction(itemId, ActionType.RequestClaim, request.Message);

            return Ok(new SuccessResponse<ItemDTO>(null, _mapper.Map<ItemDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("found/{itemId}/claimed")]
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

    [HttpGet]
    [Route("found")]
    public async Task<ActionResult<SuccessResponse<ICollection<FoundItemDTO>>>> FindAllApprovedFoundItems()
    {
        try
        {
            var result = await _itemService.FindAllApprovedFoundItems();

            return Ok(new SuccessResponse<ICollection<FoundItemDTO>>(null,
                _mapper.Map<ICollection<FoundItemDTO>>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
    
    [HttpGet]
    [Route("found/pending")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ICollection<FoundItemDTO>>>> FindAllPendingFoundItems()
    {
        try
        {
            var result = await _itemService.FindAllAPendingFoundItems();

            return Ok(new SuccessResponse<ICollection<FoundItemDTO>>(null,
                _mapper.Map<ICollection<FoundItemDTO>>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }
    
    [HttpGet]
    [Route("found/request-claim/pending")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ICollection<PendingFoundItemRequestClaim>>>> FindAllPendingFoundItemRequestClaims()
    {
        try
        {
            var result = await _itemService.FindAllPendingFoundItemRequestClaims();

            return Ok(new SuccessResponse<ICollection<PendingFoundItemRequestClaim>>(null,
                _mapper.Map<ICollection<PendingFoundItemRequestClaim>>(result)));
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
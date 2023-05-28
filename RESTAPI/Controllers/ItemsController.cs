using System.Security.Claims;
using Application.DAOs.Item;
using Application.DTOs.Item;
using Application.DTOs.ItemActions;
using Application.DTOs.Status;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Repositories;
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
public class ItemsController : BaseController<Domain.Entities.Item, IItemRepository, Application.DTOs.Item.InsertItemRequest, Application.DTOs.Item.UpdateItemRequest, Application.DTOs.Item.ItemDTO>
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ItemService _itemService;
    private readonly ItemActionsService _itemActionsService;

    public ItemsController(IItemRepository repository, IMapper mapper, IConfiguration configuration, ItemService itemService,
        ItemActionsService itemActionsService) : base(repository, mapper)
    {
        _mapper = mapper;
        _configuration = configuration;
        _itemService = itemService;
        _itemActionsService = itemActionsService;
    }

    [HttpPost]
    [Route("request-found")]
    public async Task<ActionResult<SuccessResponse<ItemDTO>>> RequestFoundItem(
        [FromForm] InsertFoundItemRequest request)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);

            var result = await _itemService.RequestFoundItem(request, userIdentity.Id);

            var data = SetImagePath(_mapper.Map<ItemDTO>(result));

            return Ok(new SuccessResponse<ItemDTO>(null, data));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPatch]
    [Route("request-found/{requestId}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<StatusDTO>>> ApproveRequestFoundItem([FromRoute] Guid requestId,
        [FromBody] ApproveFoundItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.ApproveItemAction(requestId, request.Message);

            return Ok(new SuccessResponse<StatusDTO>(null, _mapper.Map<StatusDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPatch]
    [Route("request-found/{requestId}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<StatusDTO>>> RejectRequestFoundItem([FromRoute] Guid requestId,
        [FromBody] RejectFoundItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.RejectItemAction(requestId, request.Message);

            return Ok(new SuccessResponse<StatusDTO>(null, _mapper.Map<StatusDTO>(result)));
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

            var data = SetImagePath(_mapper.Map<ItemDTO>(result));

            return Ok(new SuccessResponse<ItemDTO>(null, data));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPatch]
    [Route("found/request-claim/{requestId}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<StatusDTO>>> ApproveRequestClaimItem([FromRoute] Guid requestId,
        ApproveRequestClaimItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.ApproveItemAction(requestId, request.Message);

            return Ok(new SuccessResponse<StatusDTO>(null, _mapper.Map<StatusDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPatch]
    [Route("found/request-claim/{requestId}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<StatusDTO>>> RejectRequestClaimItem([FromRoute] Guid requestId,
        [FromBody] RejectRequestClaimItemRequest request)
    {
        try
        {
            var result = await _itemActionsService.RejectItemAction(requestId, request.Message);

            return Ok(new SuccessResponse<StatusDTO>(null, _mapper.Map<StatusDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("request-claim/{requestId}/confirm-claimed")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> UpdateStatusClaimedItem([FromRoute] Guid requestId)
    {
        try
        {
            var result = await _itemActionsService.ConfirmStatus(requestId, ActionType.Claimed);

            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpPost]
    [Route("request-found/{requestId}/confirm-found")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ItemActionsDTO>>> UpdateStatusFoundItem([FromRoute] Guid requestId)
    {
        try
        {
            var result = await _itemActionsService.ConfirmStatus(requestId, ActionType.Found);

            return Ok(new SuccessResponse<ItemActionsDTO>(null, _mapper.Map<ItemActionsDTO>(result)));
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

            var data = SetImagePath(_mapper.Map<IList<FoundItemDTO>>(result));

            return Ok(new SuccessResponse<ICollection<FoundItemDTO>>(null, data));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpGet]
    [Route("request-found")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuccessResponse<ICollection<RequestFoundItem>>>> FindAllRequestFoundItems(
        [FromQuery] ActionRequestQuery query)
    {
        try
        {
            var result = await _itemService.FindAllRequestFoundItems(query);
            
            foreach (var r in result)
            {
                r.RequestItem.ImagePath = $"{GetImagePath(HttpContext)}/{r.RequestItem.ImagePath}";
            }

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
    public async Task<ActionResult<SuccessResponse<ICollection<FoundItemRequestClaim>>>> FindAllFoundItemRequestClaims(
        [FromQuery] ActionRequestQuery query)
    {
        try
        {
            var result = await _itemService.FindAllFoundItemRequestClaims(query);
            
            foreach (var r in result)
            {
                r.RequestItem.ImagePath = $"{GetImagePath(HttpContext)}/{r.RequestItem.ImagePath}";
            }

            return Ok(new SuccessResponse<ICollection<FoundItemRequestClaim>>(null, result));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpGet]
    [Route("my/request-found")]
    public async Task<ActionResult<SuccessResponse<ICollection<MyRequestFoundResponse>>>> MyRequestFoundItems(
        [FromQuery] ActionRequestQuery query)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);

            var result = await _itemActionsService.MyRequestFoundItems(userIdentity.Id, query);

            var data = _mapper.Map<ICollection<MyRequestFoundResponse>>(result);
            
            foreach (var d in data)
            {
                d.Item.ImagePath = $"{GetImagePath(HttpContext)}/{d.Item.ImagePath}";
            }

            return Ok(new SuccessResponse<ICollection<MyRequestFoundResponse>>(null, data));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [HttpGet]
    [Route("my/request-claim")]
    public async Task<ActionResult<SuccessResponse<ICollection<MyRequestClaimResponse>>>> MyRequestClaimItems(
        [FromQuery] ActionRequestQuery query)
    {
        try
        {
            var userIdentity = GetUserIdentityFromClaims(User);

            var result = await _itemActionsService.MyRequestClaimItems(userIdentity.Id, query);
            
            var data = _mapper.Map<ICollection<MyRequestClaimResponse>>(result);
            
            foreach (var d in data)
            {
                d.Item.ImagePath = $"{GetImagePath(HttpContext)}/{d.Item.ImagePath}";
            }

            return Ok(new SuccessResponse<ICollection<MyRequestClaimResponse>>(null, data));
        }
        catch (ServiceException e)
        {
            return StatusCode((int)e.ErrorType, new FailResponse<string>(e.Message, (int)e.ErrorType));
        }
    }

    [NonAction]
    private UserIdentity GetUserIdentityFromClaims(ClaimsPrincipal claimsPrincipal)
    {
        var id = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.Sid));
        var nik = claimsPrincipal.FindFirstValue(ClaimTypes.PrimarySid);
        var roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return new UserIdentity(id, nik, roles);
    }

    [NonAction]
    private T SetImagePath<T>(T data)
        where T : ItemDTO
    {
        var baseUrl =
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
        var photosPath = _configuration["Application:ItemPhotosPath"].Substring(1);

        data.ImagePath = $"{baseUrl}{photosPath}{data.ImagePath}";

        return data;
    }

    [NonAction]
    private ICollection<T> SetImagePath<T>(IList<T> data)
        where T : ItemDTO
    {
        for (int i = 0; i < data.Count; i++)
        {
            data[i] = SetImagePath(data[i]);
        }

        return data;
    }

    [NonAction]
    private string GetImagePath(HttpContext context)
    {
        var baseUrl =
            $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
        var imagePath = _configuration["Application:ItemPhotosPath"].Substring(1);

        return baseUrl + imagePath;
    }
}
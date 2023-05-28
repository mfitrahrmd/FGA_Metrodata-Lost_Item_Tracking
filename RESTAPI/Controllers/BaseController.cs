#region

using System.Net;
using Application.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace RESTAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TEntity, TRepository, TInsert, TUpdate, TDTO> : ControllerBase
    where TEntity : BaseEntity
    where TRepository : IBaseRepository<TEntity>
    where TInsert : class
    where TUpdate : class
    where TDTO : class
{
    protected readonly TRepository Repository;
    private readonly IMapper _mapper;
    
    public BaseController(TRepository repository, IMapper mapper)
    {
        Repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<ICollection<TDTO>>>> FindAllAsync()
    {
        try
        {

            var entities = await Repository.FindAllAsync();

            return Ok(new SuccessResponse<ICollection<TDTO>>(null, _mapper.Map<ICollection<TDTO>>(entities)));
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new FailResponse<string>("Unexpected server error."));
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<SuccessResponse<TDTO>>> FindOneByIdAsync([FromRoute] Guid id)
    {
        try
        {
            var entity = await Repository.FindOneByIdAsync(id);

            if (entity is null)
                return NotFound(new FailResponse<string>($"{typeof(TEntity).Name} was not found with given id."));

            return Ok(new SuccessResponse<TDTO>(null, _mapper.Map<TDTO>(entity)));
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new FailResponse<string>("Unexpected server error."));
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<SuccessResponse>> DeleteOneByIdAsync([FromRoute] Guid id)
    {
        try
        {
            await Repository.DeleteOneByIdAsync(id);

            return Ok(new SuccessResponse($"{typeof(TEntity).Name} successfully deleted."));
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new FailResponse<string>("Unexpected server error."));
        }
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse>> InsertOneAsync([FromBody] TInsert data)
    {
        try
        {
            await Repository.InsertOneAsync(_mapper.Map<TEntity>(data));

            return Ok(new SuccessResponse($"{typeof(TEntity).Name} successfully inserted."));
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new FailResponse<string>("Unexpected server error."));
        }
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<SuccessResponse>> UpdateOneAsync([FromRoute] Guid id, [FromBody] TUpdate data)
    {
        try
        {
            await Repository.UpdateOneByIdAsync(id, _mapper.Map<TEntity>(data));

            return Ok(new SuccessResponse($"{typeof(TEntity).Name} successfully updated."));
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new FailResponse<string>("Unexpected server error."));
        }
    }
}
#region

using Application.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace RESTAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TEntity, TRepository, TDTO, TInsertOneReq> : ControllerBase
    where TEntity : BaseEntity
    where TRepository : IBaseRepository<TEntity>
    where TDTO : class
    where TInsertOneReq : class
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
        var entities = await Repository.FindAllAsync();

        return Ok(new SuccessResponse<ICollection<TDTO>>(null, _mapper.Map<ICollection<TDTO>>(entities)));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<SuccessResponse<TDTO>>> FindOneByIdAsync([FromRoute] Guid id)
    {
        var entity = await Repository.FindOneByIdAsync(id);

        if (entity is null)
            return NotFound(new FailResponse<string>($"{nameof(TEntity)} was not found with given id."));

        return Ok(new SuccessResponse<TDTO>(null, _mapper.Map<TDTO>(entity)));
    }
}
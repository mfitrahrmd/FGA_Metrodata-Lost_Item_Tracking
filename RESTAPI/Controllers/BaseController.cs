using Application.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

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
}
using Application.Repositories;
using AutoMapper;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

public abstract class BaseController<TEntity, TRepository, TDTO, TInsertOneReq> : ControllerBase
    where TEntity : BaseEntity
    where TRepository : IBaseRepository<TEntity>
    where TDTO : class
    where TInsertOneReq : class
{
    private readonly TRepository _repository;
    private readonly IMapper _mapper;
    
    public BaseController(TRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}
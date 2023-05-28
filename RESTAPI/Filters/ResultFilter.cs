using Application.DTOs.Item;
using Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RESTAPI.Filters;

public class ResultFilter : ResultFilterAttribute
{
    private readonly IConfiguration _configuration;

    public ResultFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}
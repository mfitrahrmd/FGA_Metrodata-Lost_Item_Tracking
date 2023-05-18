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
    
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var ok = context.Result as ObjectResult;

        if (ok != null)
        {
            var data = ok.Value as SuccessResponse<ItemDTO>;

            if (data != null)
            {
                var baseUrl =
                    $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.PathBase}";
                var photosPath = _configuration["Application:ItemPhotosPath"].Substring(1);
                data.Data.ImagePath = $"{baseUrl}{photosPath}{data.Data.ImagePath}";
            }
        }
    }
}
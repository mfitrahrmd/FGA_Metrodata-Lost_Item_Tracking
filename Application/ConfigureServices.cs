#region

using System.Reflection;
using Application.Services.Account;
using Application.Services.Item;

#endregion

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<AccountService>();
        services.AddScoped<ItemService>();

        return services;
    }
}
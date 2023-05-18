#region

using System.Reflection;
using Application.Services.Account;
using Application.Services.Item;
using Application.Services.ItemActions;

#endregion

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<AccountService>();
        services.AddScoped<ItemService>();
        services.AddScoped<ItemActionsService>();

        return services;
    }
}
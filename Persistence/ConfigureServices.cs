#region

using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;

#endregion

namespace Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
            builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)).UseSnakeCaseNamingConvention());

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IActionRepository, ActionRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IItemActionsRepository, ItemActionsRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        
        return services;
    }
}

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MotoVerse.Infrastructure;

public static class ModuleInfrastructureDependencies
{
    public static IServiceCollection InfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure SQL Server
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        // Apply Redis Connection : 
        services.AddSingleton<IConnectionMultiplexer>(i =>
        {
            var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));

            return ConnectionMultiplexer.Connect(config);

        });

        //AddScoped Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        //AddScoped Repository Manager   
        services.AddScoped<IRepositoryManager, RepositoryManager>();


        return services;
    }
}

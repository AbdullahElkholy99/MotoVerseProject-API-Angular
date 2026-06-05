using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using MotoVerse.Core.Behaviors;
using System.Globalization;

namespace MotoVerse.Core;

public static class ModuleCoreDependencies
{
    public static IServiceCollection CoreDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // AddMediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        //Configuration Of Automapper
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Get Validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // 
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("ar-EG")
        };

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en-US");

            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders = new[]
            {
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });

        return services;
    }
}

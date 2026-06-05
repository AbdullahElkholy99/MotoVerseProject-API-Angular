using Hangfire;
using LegalFlow.API.ServiceExtentions;
using MotoVerse.API.Attributes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Swagger
builder.Services.AddEndpointsApiExplorer();

#region DI

builder.Services
    .RegisterSwaggerGen(builder.Configuration)
    .InfrastructureDependencies(builder.Configuration)
    .CoreDependencies(builder.Configuration)
    .EnableLocalization()
    .EnableIUrlHelper()
    .AddHttpContextAccessor()
    .EnableCORS()
    .AddHangfire(builder.Configuration)
    .AddServiceAuthentication(builder.Configuration)
    .AddServiceAuthorization(builder.Configuration)
    .AddServiceRegisteration(builder.Configuration);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Localization Middleware
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
#endregion

// 3. Mount the dashboard UI
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireAuthorizationFilter()]
});

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenMiddleware>();

app.MapControllers();

app.Run();
public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString();
        Console.WriteLine("--------------------------------------");
        Console.WriteLine($"TOKEN: {token}");
        Console.WriteLine("--------------------------------------");

        await _next(context);
    }
}


/*
 login : 
    elkholyA23@gmail.com            -           elkholyA23@gmail.com
    abdullah.ali.elkholy@gmail.com  -  A32bdullah.ali.elkholy@gmail.com
 
 */
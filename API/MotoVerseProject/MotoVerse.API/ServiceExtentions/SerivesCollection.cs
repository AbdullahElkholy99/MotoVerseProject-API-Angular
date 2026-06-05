
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LegalFlow.API.ServiceExtentions;

public static class SerivesCollection
{
    //EnableLocalization
    public static IServiceCollection EnableLocalization(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddLocalization(opt =>
        {
            opt.ResourcesPath = "";
        });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            List<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("ar-EG")
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        return services;
    }

    //EnableCORS
    public static IServiceCollection EnableCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:4200",    // Angular dev server
                        "https://myapp.com"        // Production domain
                    )
                    .AllowAnyMethod()              // GET, POST, PUT, PATCH, DELETE
                    .AllowAnyHeader()              // Content-Type, Authorization, etc.
                    .AllowCredentials();           // Allow cookies/auth headers
            });
        });

        return services;
    }

    //EnableIUrlHelper
    public static IServiceCollection EnableIUrlHelper(this IServiceCollection services)
    {
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.AddScoped<IUrlHelper>(serviceProvider =>
        {
            var actionContext = serviceProvider
                .GetRequiredService<IActionContextAccessor>()
                .ActionContext;

            var factory = serviceProvider.GetRequiredService<IUrlHelperFactory>();

            return factory.GetUrlHelper(actionContext);
        });
        return services;
    }


    //add hangfire
    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
     config
         // بيحدد طريقة تخزين البيانات بحيث تكون متوافقة مع النسخة
         .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
         //دي مسؤولة عن:
         // إزاي Hangfire يحول الـ Jobs لبيانات تتخزن في DB
         // ببساطة:
         //- يحفظ الميثود + البراميترز
         //- ويرجع يشغلهم تاني بعدين
         .UseSimpleAssemblyNameTypeSerializer()
         .UseRecommendedSerializerSettings()
         //  “Store All Jobs in SQL Server”
         .UseSqlServerStorage(
         configuration.GetConnectionString("DefaultConnection"),
             new SqlServerStorageOptions
             {
                 // أقصى وقت لتنفيذ batch من الأوامر
                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                 //لو Worker مسك Job ومكملش (crash)  
                 //→ بعد 5 دقايق يرجع يتنفذ تاني
                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                 //يخلي Hangfire يسحب Jobs بسرعة جدًا (بدون delay)
                 QueuePollInterval = TimeSpan.Zero,
                 UseRecommendedIsolationLevel = true,
                 DisableGlobalLocks = true
             }
         )
 );

        // 2. Register the Hangfire processing server
        services.AddHangfireServer(options =>
         {
             options.WorkerCount = 5; // number of parallel workers (Worker = Thread)
         });

        return services;
    }

    public static IServiceCollection AddServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, Role>(option =>
        {

            // Password settings.
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = true;
            option.Password.RequireNonAlphanumeric = true;
            option.Password.RequireUppercase = true;
            option.Password.RequiredLength = 6;
            option.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            // User settings.
            option.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            //option.SignIn.RequireConfirmedEmail = true;

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        //JWT Authentication
        var jwtSettings = new JwtSettings();
        var emailSettings = new EmailSettings();

        configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);

        services.AddSingleton(jwtSettings);
        services.AddSingleton(emailSettings);


        return services;
    }

    //AddSwaggerGen
    public static IServiceCollection RegisterSwaggerGen(this IServiceCollection services, IConfiguration configuration)
    {

        //Swagger Gn
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "LegalFlow Project", Version = "v1" });
            //c.EnableAnnotations();

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            //c.AddSecurityRequirement(new OpenApiSecurityRequirement
            // {
            //     {
            //     new OpenApiSecurityScheme
            //     {
            //         Reference = new OpenApiReference
            //         {
            //             Type = ReferenceType.SecurityScheme,
            //             Id = JwtBearerDefaults.AuthenticationScheme
            //         }
            //     },
            //     Array.Empty<string>()
            //     }
            //});


        });

        return services;
    }

    //AddServiceAuthentication
    public static IServiceCollection AddServiceAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        var jwtKey = configuration["jwtSettings:secret"]!;

        Console.WriteLine("============ JWT KEY:");
        Console.WriteLine(jwtKey);
        services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("------------------------------ ERROR:");
                        Console.WriteLine(context.Exception.Message);

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        Console.WriteLine(" --------------------------------- TOKEN VALID");

                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                                                  Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidIssuer = configuration["jwtSettings:Issuer"],
                    ValidateAudience = false,
                    ValidAudience = configuration["jwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero    // No grace period
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        Console.WriteLine("--------------------------------TOKEN RECEIVED");
                        Console.WriteLine("AUTH HEADER:");
                        Console.WriteLine(
                            context.Request.Headers["Authorization"].ToString());


                        var authHeader = context.Request.Headers["Authorization"].ToString();

                        if (!string.IsNullOrEmpty(authHeader) &&
                            authHeader.StartsWith("Bearer "))
                        {
                            context.Token = authHeader["Bearer ".Length..].Trim();
                        }
                        Console.WriteLine("\t\tTOKEN:");
                        Console.WriteLine(context.Token ?? "NULL");

                        Console.WriteLine("-------------------------------- RECEIVED");

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("TOKEN VALIDATED");
                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("AUTH FAILED");
                        Console.WriteLine(context.Exception.ToString());
                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        Console.WriteLine("CHALLENGE");
                        Console.WriteLine($"Error: {context.Error}");
                        Console.WriteLine($"Description: {context.ErrorDescription}");
                        return Task.CompletedTask;
                    }
                };
            })
             .AddGoogle("Google", options =>
             {
                 var googleAuthSection = configuration.GetSection("Authentication:Google");
                 options.ClientId = googleAuthSection["ClientId"]!;
                 options.ClientSecret = googleAuthSection["ClientSecret"]!;
                 options.Scope.Add("profile");
             })
             .AddFacebook("Facebook", options =>
             {
                 var googleAuthSection = configuration.GetSection("Authentication:Facebook");
                 options.AppId = googleAuthSection["AppId"]!;
                 options.AppSecret = googleAuthSection["AppSecret"]!;
                 options.Scope.Add("email");
                 options.Scope.Add("public_profile");
             });
        return services;

    }
    //AddServiceAuthorization
    public static IServiceCollection AddServiceAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(option =>
        {
            option.AddPolicy("CreateCategory", policy =>
            {
                policy.RequireClaim("Create Category", "True");
            });
            option.AddPolicy("DeleteCategory", policy =>
            {
                policy.RequireClaim("Delete Category", "True");
            });
            option.AddPolicy("EditCategory", policy =>
            {
                policy.RequireClaim("Edit Category", "True");
            });
        });



        return services;
    }






}

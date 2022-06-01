using AutoMapper;

using BookSearch.API.Contexts;
using BookSearch.API.DDD.External;
using BookSearch.API.DDD.Favorite;
using BookSearch.API.DDD.PasswordHasher;
using BookSearch.API.DDD.PasswordRecovery;
using BookSearch.API.DDD.Person;
using BookSearch.API.DDD.RefreshToken;
using BookSearch.API.DDD.Token;
using BookSearch.API.DDD.User;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;
using System.Text.Json.Serialization;

namespace BookSearch.API.Startup;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var config = new ConfigurationReader(builder.Configuration);
        var services = builder.Services;

        ConfigureDatabase(services, config.DefaultConnectionString);
        AddController(services);
        AddAuthentication(services, config.Info, config.Security.TokenSecurity);
        AddSwagger(services, config.Info);
        AddAutomapper(services);
        ConfigureDependencyInjection(services);

        return builder;
    }

    private static void AddAutomapper(IServiceCollection services)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<FavoriteModel, FavoriteDTO>().ReverseMap();
        });

        var mapper = config.CreateMapper();

        services.AddSingleton(mapper);
    }

    private static void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddDbContext<DefaultContext>();

        services.AddSingleton<ConfigurationReader>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IPersonRepository, PersonRepository>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddTransient<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
        services.AddTransient<ISmtpService, SmtpService>();
        services.AddTransient<IFavoriteRepository, FavoriteRepository>();
    }

    private static void AddController(IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = false;
                    options.JsonSerializerOptions.MaxDepth = 0;
                    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
                    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                });
    }

    private static void AddSwagger(IServiceCollection services, ConfigurationInfo info)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = info.ProductName,
                Version = info.Version,
                Description = info.Name,
                Contact = new OpenApiContact
                {
                    Name = info.Name,
                    Email = info.Email,
                }
            });

            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
    }

    private static void AddAuthentication(IServiceCollection services, ConfigurationInfo info, string tokenSecurity)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = info.Site,
                    ValidAudience = info.Site,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurity))
                });
    }

    private static void ConfigureDatabase(IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        services.AddDbContext<DefaultContext>(options =>
        {
            options.UseNpgsql(connectionString)
                        .UseSnakeCaseNamingConvention();
        });

    }
}

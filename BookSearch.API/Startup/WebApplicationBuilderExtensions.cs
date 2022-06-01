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

        ConfigureDatabase(builder, config.DefaultConnectionString);
        AddController(builder);
        AddAuthentication(builder, config.Info, config.Security.TokenSecurity);
        AddSwagger(builder, config.Info);
        ConfigureDependencyInjection(builder);

        return builder;
    }

    private static void ConfigureDependencyInjection(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DefaultContext>();

        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IPersonRepository, PersonRepository>();
        builder.Services.AddTransient<ITokenService, TokenService>();
        builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddTransient<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
        builder.Services.AddTransient<ISmtpService, SmtpService>();
        builder.Services.AddTransient<IFavoriteRepository, FavoriteRepository>();
    }

    private static void AddController(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = false;
                    options.JsonSerializerOptions.MaxDepth = 0;
                    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
                    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                });
    }

    private static void AddSwagger(WebApplicationBuilder builder, ConfigurationInfo info)
    {

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(config =>
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

    private static void AddAuthentication(WebApplicationBuilder builder, ConfigurationInfo info, string tokenSecurity)
    {
        builder.Services.AddAuthentication(options =>
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

    private static void ConfigureDatabase(WebApplicationBuilder builder, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        builder.Services.AddDbContext<DefaultContext>(options =>
        {
            options.UseNpgsql(connectionString)
                        .UseSnakeCaseNamingConvention();
        });

    }
}


using BookSearch.API.Contexts;
using BookSearch.API.DDD.External;
using BookSearch.API.DDD.PasswordHasher;
using BookSearch.API.DDD.PasswordRecovery;
using BookSearch.API.DDD.RefreshToken;
using BookSearch.API.DDD.Token;
using BookSearch.API.DDD.User;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Text.Json.Serialization;
using BookSearch.API.DDD.Person;
using BookSearch.API.DDD.Favorite;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = false;
                    options.JsonSerializerOptions.MaxDepth = 0;
                    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
                    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                });
var currentAssembly = typeof(Program).Assembly;

builder.Services.AddDbContext<DefaultContext>(options => options.UseNpgsql(
        connectionString
        )
    .UseSnakeCaseNamingConvention()
);

builder.Services.AddEndpointsApiExplorer();
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
                    ValidIssuer = builder.Configuration["Info:Site"],
                    ValidAudience = builder.Configuration["Info:Site"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSecurity"]))
                });
builder.Services.AddSwaggerGen(config =>
{
    var apiName = builder.Configuration["Info.ProductName"];

    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = apiName,
        Version = builder.Configuration["Info:Version"],
        Description = apiName,
        Contact = new OpenApiContact
        {
            Name = builder.Configuration["Info:Name"],
            Email = builder.Configuration["Info.Email"],
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

builder.Services.AddDbContext<DefaultContext>();

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddTransient<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
builder.Services.AddTransient<ISmtpService, SmtpService>();
builder.Services.AddTransient<IFavoriteRepository, FavoriteRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(config =>
{
    config.AllowAnyHeader();
    config.AllowAnyMethod();
    config.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
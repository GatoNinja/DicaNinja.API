using AutoMapper;

using DicaNinja.API.Contexts;

using DicaNinja.API.Models;
using DicaNinja.API.Providers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using DicaNinja.API.Response;
using DicaNinja.API.Services;
using DicaNinja.API.Validations;

using Google.Apis.Books.v1.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;
using System.Text.Json.Serialization;

namespace DicaNinja.API.Startup;

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
        AddAutoMapper(services);
        AddHttpLogging(services);
        ConfigureDependencyInjection(services);

        return builder;
    }

    private static void AddHttpLogging(IServiceCollection services)
    {
        services.AddHttpLogging(config =>
        {
            config.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath | HttpLoggingFields.Response;
        });
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<Volume, BookResponse>()
                .ForMember(dest => dest.PageCount, opt => opt.MapFrom(src => src.VolumeInfo.PageCount ?? 0))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.VolumeInfo.PublishedDate ?? string.Empty))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetImage(src)))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.VolumeInfo.AverageRating ?? 0))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.VolumeInfo.Title))
                .ForMember(dest => dest.Subtitle, opt => opt.MapFrom(src => src.VolumeInfo.Subtitle))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.VolumeInfo.Language))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.VolumeInfo.Categories))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.VolumeInfo.Description))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.VolumeInfo.Publisher))
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.VolumeInfo.Authors))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            config.CreateMap<IdentifierResponse, Identifier>()
                .ReverseMap();

            config.CreateMap<BookResponse, Book>()
                .ReverseMap();

            config.CreateMap<User, UserResponse>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName));

            config.CreateMap<Person, PersonRequest>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            config.CreateMap<Person, PersonResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

        });

        var mapper = config.CreateMapper();

        services.AddSingleton(mapper);
    }

    private static void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddDbContext<BaseContext>();

        services.AddSingleton<ConfigurationReader>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IUserProvider, UserProvider>();
        services.AddTransient<IPersonProvider, PersonProvider>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IRefreshTokenProvider, RefreshTokenProvider>();
        services.AddTransient<IPasswordRecoveryProvider, PasswordRecoveryProvider>();
        services.AddTransient<ISmtpService, SmtpService>();
        services.AddTransient<IBookmarkProvider, BookmarkProvider>();
        services.AddTransient<IBookProvider, BookProvider>();
        services.AddTransient<IFollowerProvider, FollowerProvider>();
        services.AddTransient<IIdentifierProvider, IdentifierProvider>();
        services.AddTransient<IAuthorProvider, AuthorProvider>();
        services.AddTransient<ICategoryProvider, CategoryProvider>();
        services.AddTransient<IReviewProvider, ReviewProvider>();
        services.AddTransient<IProfileProvider, ProfileProvider>();
        services.AddTransient<BookGoogleService>();
    }

    private static void AddController(IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add(new ValidateModelFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.AllowTrailingCommas = false;
                options.JsonSerializerOptions.MaxDepth = 0;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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

        services.AddDbContext<BaseContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }

    private static string GetImage(Volume src)
    {
        return src.VolumeInfo.ImageLinks?.Thumbnail is null
            ? src.VolumeInfo.ImageLinks?.Thumbnail ?? string.Empty
            : src.VolumeInfo.ImageLinks?.SmallThumbnail ?? string.Empty;
    }

}

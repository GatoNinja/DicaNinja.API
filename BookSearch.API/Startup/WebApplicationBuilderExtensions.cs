using AutoMapper;

using BookSearch.API.Contexts;

using Google.Apis.Books.v1.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using BookSearch.API.Response;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Repository;
using BookSearch.API.Services;

namespace BookSearch.API.Startup
{
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

        private static void AddAutomapper(IServiceCollection services)
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

                config.CreateMap<IdentifierDTO, Identifier>()
                    .ReverseMap();

                config.CreateMap<BookResponse, Book>()
                    .ReverseMap();
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
            services.AddTransient<IBookmarkRepository, BookmarkRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IIdentifierRepository, IdentifierRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<BookGoogleService>();
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

        private static string GetImage(Volume src)
        {
            if (src.VolumeInfo.ImageLinks?.Thumbnail != null)
                return src.VolumeInfo.ImageLinks.Thumbnail;

            if (src.VolumeInfo.ImageLinks?.SmallThumbnail != null)
                return src.VolumeInfo.ImageLinks.SmallThumbnail;

            return string.Empty;
        }

    }
}

using DicaNinja.API.Startup;

namespace DicaNinja.API;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureServices();

        var app = builder.Build();
        app.UseHttpLogging();

        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.EnablePersistAuthorization();
            config.EnableValidator();
            config.EnableFilter();
        });

        app.UseDeveloperExceptionPage();

        app.UseMiddleware<RequestLoggingMiddleware>();

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
    }
}

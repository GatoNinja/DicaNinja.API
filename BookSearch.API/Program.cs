using BookSearch.API.Startup;

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

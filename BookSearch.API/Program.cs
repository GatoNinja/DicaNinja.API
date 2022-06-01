using BookSearch.API.Startup;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureServices();

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

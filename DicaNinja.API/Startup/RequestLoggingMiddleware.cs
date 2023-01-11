namespace DicaNinja.API.Startup;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        finally
        {
            _logger.LogInformation(
                  "{Date} Request {Method} {Url} {Params} {Ip} => {StatusCode}",
                  DateTimeOffset.Now,
                  context.Request?.Method,
                  context.Request?.Path.Value,
                  context.Request?.QueryString,
                  context.Connection?.RemoteIpAddress?.ToString(),
                  context.Response?.StatusCode);
        }
    }
}

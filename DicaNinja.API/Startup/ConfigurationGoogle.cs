namespace DicaNinja.API.Startup;

public class ConfigurationGoogle
{
    public string? ApiKey { get; set; }
    public string? Application { get; set; }

    public ConfigurationGoogle(string? apiKey, string? application)
    {
        ApiKey = apiKey;
        Application = application;
    }
}

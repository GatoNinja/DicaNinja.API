namespace DicaNinja.API.Startup;

public class ConfigurationReader
{
    public ConfigurationInfo Info { get; set; }

    public string? DefaultConnectionString { get; set; }

    public ConfigurationSecurity Security { get; set; }

    public ConfigurationGoogle Google { get; set; }

    public ConfigurationReader(IConfiguration config)
    {
        if (config is null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        DefaultConnectionString = config.GetConnectionString("DefaultConnection");
        Info = new ConfigurationInfo(config["Info:Site"], config["Info:Email"], config["Info:Name"], config["Info:Name"], config["Info:Version"]);
        Security = new ConfigurationSecurity(config["TokenSecurity"], config.GetValue<int>("TokenExpiryInMinutes"), config.GetValue<int>("HashIterations"), config["DefaultUserRole"]);
        Google = new ConfigurationGoogle(config["Google:ApiKey"], config["Google:Application"]);
    }
}

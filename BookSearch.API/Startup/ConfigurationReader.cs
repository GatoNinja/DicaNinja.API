namespace BookSearch.API.Startup;

public class ConfigurationReader
{
    public ConfigurationInfo Info { get; set; }

    public string DefaultConnectionString { get; set; }

    public ConfigurationSecurity Security { get; set; } 

    public ConfigurationReader(IConfiguration config)
    {
        Info = new ConfigurationInfo(config["Info:Site"], config["Info:Email"], config["Info:Name"], config["Info:Name"], config["Info:Version"]);
        Security = new ConfigurationSecurity(config["TokenSecurity"], config.GetValue<int>("TokenExpiryInMinutes"), config.GetValue<int>("HashIterations"), config["DefaultUserRole"]);
        DefaultConnectionString = config.GetConnectionString("DefaultConnection");
    }
}

public record ConfigurationInfo(string Site, string Email, string Name, string ProductName, string Version);

public record ConfigurationSecurity (string TokenSecurity, int TokenExpiryInMinutes, int HashIterations, string DefaultUserRole);

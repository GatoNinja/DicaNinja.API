namespace DicaNinja.API.Startup;

public class ConfigurationInfo
{
    public string? Site { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? ProductName { get; set; }
    public string? Version { get; set; }

    public ConfigurationInfo(string? site, string? email, string? name, string? productName, string? version)
    {
        Site = site;
        Email = email;
        Name = name;
        ProductName = productName;
        Version = version;
    }
}

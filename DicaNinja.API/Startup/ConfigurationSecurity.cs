namespace DicaNinja.API.Startup;

public class ConfigurationSecurity
{
    public string TokenSecurity { get; set; }
    public int TokenExpiryInMinutes { get; set; }
    public int HashIterations { get; set; }
    public string DefaultUserRole { get; set; }

    public ConfigurationSecurity(string tokenSecurity, int tokenExpiryInMinutes, int hashIterations, string defaultUserRole)
    {
        TokenSecurity = tokenSecurity;
        TokenExpiryInMinutes = tokenExpiryInMinutes;
        HashIterations = hashIterations;
        DefaultUserRole = defaultUserRole;
    }
}

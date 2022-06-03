namespace DicaNinja.API.Startup;

public record ConfigurationSecurity(string TokenSecurity, int TokenExpiryInMinutes, int HashIterations, string DefaultUserRole);
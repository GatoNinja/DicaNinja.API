using DicaNinja.API.Startup;

using System.Security.Cryptography;

namespace DicaNinja.API.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public PasswordHasher(ConfigurationReader config)
    {
        this.Iterations = Convert.ToInt32(config.Security.HashIterations);
    }

    private int Iterations { get; }

    public string Hash(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize, this.Iterations);
        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{this.Iterations}.{salt}.{key}";
    }

    public bool Check(string hash, string password)
    {
        var parts = hash.Split('.');

        if (parts.Length != 3)
        {
            throw new FormatException("Unexpected hash format. " +
                                      "Should be formatted as `{iterations}.{salt}.{hash}`");
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations);
        var keyToCheck = algorithm.GetBytes(KeySize);

        var verified = keyToCheck.SequenceEqual(key);

        return verified;
    }
}
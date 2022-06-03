namespace DicaNinja.API.Services;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Check(string hash, string password);
}
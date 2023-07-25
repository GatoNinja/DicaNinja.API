using System.Runtime.Caching;

namespace DicaNinja.API.Cache;

public class CacheService : ICacheService
{
    private readonly ObjectCache _memoryCache = MemoryCache.Default;

    public T GetData<T>(string key)
    {
        try
        {
            var item = (T)_memoryCache.Get(key);

            return item;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var res = true;

        try
        {
            if (!string.IsNullOrEmpty(key) && value is not null)
            {
                _memoryCache.Set(key, value, expirationTime);
            }
        }
        catch (Exception)
        {
            throw;
        }

        return res;
    }
    public object RemoveData(string key)
    {
        try
        {
            if (!string.IsNullOrEmpty(key))
            {
                return _memoryCache.Remove(key);
            }
        }
        catch (Exception)
        {
            throw;
        }
        return false;
    }
}

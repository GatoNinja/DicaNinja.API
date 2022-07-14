namespace DicaNinja.API.Helpers;

public class Response<T>
{
    public T Data { get; }

    public Response(T data)
    {
        Data = data;
    }
}

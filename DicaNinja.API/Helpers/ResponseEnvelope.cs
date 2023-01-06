namespace DicaNinja.API.Helpers;

public class ResponseEnvelope<T>
{
    public T Data { get; }

    public ResponseEnvelope(T data)
    {
        Data = data;
    }
}

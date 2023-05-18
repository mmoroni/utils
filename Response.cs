namespace POPriceUpdates.Core.Monads;
public class Response<T> : Response
{
    private readonly T _value;

    public Response(T value)
    {
        _value = value;
    }
}
public class Response
{
    public static Response Success => new();

    public static Response Failure()
    {
        return new Response();
    }

    public static Response<T> Failure<T>(T value)
    {
        return new Response<T>(value);
    }
}

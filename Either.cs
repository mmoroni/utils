using System.Diagnostics;

namespace POPriceUpdates.Core.Monads;

[DebuggerStepThrough]
public class Either<Success, Error>
{
    private readonly bool _isFailure;
    private readonly Success _success;
    private readonly Error _error;

    public static implicit operator Either<Success, Error>(Success success)
    {
        return new Either<Success, Error>(success);
    }

    public static implicit operator Either<Success, Error>(Error error)
    {
        return new Either<Success, Error>(error);
    }

    private Either(Success left)
    {
        _isFailure = false;
        _success = left;
        _error = default;
    }

    private Either(Error right)
    {
        _isFailure = true;
        _error = right;
        _success = default;
    }

    public bool IsFailure()
    {
        return _isFailure;
    }

    public Error GetError()
    {
        return _isFailure
            ? _error
            : throw new InvalidOperationException();
    }

    internal Success GetSuccess()
    {
        return !_isFailure
            ? _success
            : throw new InvalidOperationException();
    }
}

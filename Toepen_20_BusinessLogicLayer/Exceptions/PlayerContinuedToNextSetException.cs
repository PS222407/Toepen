namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerContinuedToNextSetException : Exception
{
    public PlayerContinuedToNextSetException()
    {
    }

    public PlayerContinuedToNextSetException(string message)
        : base(message)
    {
    }

    public PlayerContinuedToNextSetException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

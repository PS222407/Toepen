namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class AlreadyTurnedException : Exception
{
    public AlreadyTurnedException()
    {
    }

    public AlreadyTurnedException(string message)
        : base(message)
    {
    }

    public AlreadyTurnedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
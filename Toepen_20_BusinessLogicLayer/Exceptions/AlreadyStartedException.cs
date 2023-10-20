namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class AlreadyStartedException : Exception
{
    public AlreadyStartedException()
    {
    }

    public AlreadyStartedException(string message)
        : base(message)
    {
    }

    public AlreadyStartedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
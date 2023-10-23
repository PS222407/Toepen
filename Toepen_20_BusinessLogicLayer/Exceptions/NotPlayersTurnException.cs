namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class NotPlayersTurnException : Exception
{
    public NotPlayersTurnException()
    {
    }

    public NotPlayersTurnException(string message)
        : base(message)
    {
    }

    public NotPlayersTurnException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
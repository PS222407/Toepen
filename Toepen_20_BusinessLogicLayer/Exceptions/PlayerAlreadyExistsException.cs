namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerAlreadyExistsException : Exception
{
    public PlayerAlreadyExistsException()
    {
    }

    public PlayerAlreadyExistsException(string message)
        : base(message)
    {
    }

    public PlayerAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
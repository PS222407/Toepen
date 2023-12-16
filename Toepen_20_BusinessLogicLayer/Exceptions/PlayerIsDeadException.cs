namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerIsDeadException : Exception
{
    public PlayerIsDeadException()
    {
    }

    public PlayerIsDeadException(string message)
        : base(message)
    {
    }

    public PlayerIsDeadException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
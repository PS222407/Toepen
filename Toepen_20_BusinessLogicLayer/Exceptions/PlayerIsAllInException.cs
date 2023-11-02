namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerIsAllInException : Exception
{
    public PlayerIsAllInException()
    {
    }

    public PlayerIsAllInException(string message)
        : base(message)
    {
    }

    public PlayerIsAllInException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
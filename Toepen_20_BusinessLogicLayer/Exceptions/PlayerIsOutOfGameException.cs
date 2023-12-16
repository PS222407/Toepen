namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerIsOutOfGameException : Exception
{
    public PlayerIsOutOfGameException()
    {
    }

    public PlayerIsOutOfGameException(string message)
        : base(message)
    {
    }

    public PlayerIsOutOfGameException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
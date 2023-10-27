namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerHasNotCalledForLaundryException : Exception
{
    public PlayerHasNotCalledForLaundryException()
    {
    }

    public PlayerHasNotCalledForLaundryException(string message)
        : base(message)
    {
    }

    public PlayerHasNotCalledForLaundryException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
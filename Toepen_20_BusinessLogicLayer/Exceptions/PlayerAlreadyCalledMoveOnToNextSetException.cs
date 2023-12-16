namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerAlreadyCalledMoveOnToNextSetException : Exception
{
    public PlayerAlreadyCalledMoveOnToNextSetException()
    {
    }

    public PlayerAlreadyCalledMoveOnToNextSetException(string message)
        : base(message)
    {
    }

    public PlayerAlreadyCalledMoveOnToNextSetException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

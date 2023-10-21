namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class TooManyPlayersException : Exception
{
    public TooManyPlayersException()
    {
    }

    public TooManyPlayersException(string message)
        : base(message)
    {
    }

    public TooManyPlayersException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
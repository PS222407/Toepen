namespace BusinessLogicLayer.Exceptions;

public class NotEnoughPlayersException : Exception
{
    public NotEnoughPlayersException()
    {
    }

    public NotEnoughPlayersException(string message)
        : base(message)
    {
    }

    public NotEnoughPlayersException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
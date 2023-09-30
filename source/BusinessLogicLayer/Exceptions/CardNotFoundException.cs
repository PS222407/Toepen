namespace BusinessLogicLayer.Exceptions;

public class CardNotFoundException : Exception
{
    public CardNotFoundException()
    {
    }

    public CardNotFoundException(string message)
        : base(message)
    {
    }

    public CardNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
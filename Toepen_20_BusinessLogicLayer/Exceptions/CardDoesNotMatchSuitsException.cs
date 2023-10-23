namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class CardDoesNotMatchSuitsException : Exception
{
    public CardDoesNotMatchSuitsException()
    {
    }

    public CardDoesNotMatchSuitsException(string message)
        : base(message)
    {
    }

    public CardDoesNotMatchSuitsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
namespace BusinessLogicLayer.Exceptions;

public class CantPerformToSelfException : Exception
{
    public CantPerformToSelfException()
    {
    }

    public CantPerformToSelfException(string message)
        : base(message)
    {
    }

    public CantPerformToSelfException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
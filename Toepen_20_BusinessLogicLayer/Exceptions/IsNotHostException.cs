namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class IsNotHostException : Exception
{
    public IsNotHostException()
    {
    }

    public IsNotHostException(string message)
        : base(message)
    {
    }

    public IsNotHostException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
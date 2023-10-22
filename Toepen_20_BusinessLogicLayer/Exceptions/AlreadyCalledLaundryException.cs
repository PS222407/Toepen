namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class AlreadyCalledLaundryException : Exception
{
    public AlreadyCalledLaundryException()
    {
    }

    public AlreadyCalledLaundryException(string message)
        : base(message)
    {
    }

    public AlreadyCalledLaundryException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
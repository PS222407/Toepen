namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class PlayerEmptyUserName : Exception
{
    public PlayerEmptyUserName()
    {
    }

    public PlayerEmptyUserName(string message)
        : base(message)
    {
    }

    public PlayerEmptyUserName(string message, Exception inner)
        : base(message, inner)
    {
    }
}
﻿namespace Toepen_20_BusinessLogicLayer.Exceptions;

public class InvalidStateException : Exception
{
    public InvalidStateException()
    {
    }

    public InvalidStateException(string message)
        : base(message)
    {
    }

    public InvalidStateException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
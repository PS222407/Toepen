using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class StatusMessage
{
    public bool Success { get; private set; }

    public Messages? Message { get; private set; }

    public StatusMessage(bool success)
    {
        Success = success;
    }

    public StatusMessage(bool success, Messages message)
    {
        Success = success;
        Message = message;
    }
}
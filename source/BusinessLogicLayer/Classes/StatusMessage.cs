using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class StatusMessage
{
    public bool Success { get; private set; }

    public Message? Message { get; private set; }

    public StatusMessage(bool success)
    {
        Success = success;
    }

    public StatusMessage(bool success, Message message)
    {
        Success = success;
        Message = message;
    }
}
using Toepen_20_BusinessLogicLayer.Enums;

namespace Toepen_20_BusinessLogicLayer.LogTypes;

public class LogMessage
{
    public LogType Type { get; set; }

    public string Message { get; set; }
}
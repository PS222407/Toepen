namespace Toepen_20_BusinessLogicLayer.LogTypes;

public class Log
{
    protected readonly List<LogMessage> logMessages = new();

    public IReadOnlyList<LogMessage> LogMessages => logMessages;
}

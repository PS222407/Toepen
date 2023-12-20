using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.LogTypes;

public class TurnLaundryLog : Log
{
    public TurnLaundryLog(Player victim, Player turner, bool victimBluffed)
    {
        //"{turner} turned the laundry of {victim}. {victimBluffed ? victim : turner} gets punished!";
        
        logMessages.Add(new LogMessage
        {
            Type = LogType.Player,
            Message = turner.Name,
        });
        logMessages.Add(new LogMessage
        {
            Type = LogType.Message,
            Message = " turned the laundry of ",
        });
        logMessages.Add(new LogMessage
        {
            Type = LogType.Message,
            Message = victim.Name + ".",
        });
        logMessages.Add(new LogMessage
        {
            Type = LogType.Player,
            Message = victimBluffed ? victim.Name : turner.Name,
        });
        logMessages.Add(new LogMessage
        {
            Type = LogType.Message,
            Message = "gets punished!",
        });
    }
}
using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.Helpers;

public class StatusMessage
{
    public bool Success { get; private set; }

    public Message? Message { get; private set; }
    
    public Player Winner { get; private set; }
    
    public int? RoundNumber { get; private set; }
    
    public int? SetNumber { get; private set; }

    public StatusMessage(bool success)
    {
        Success = success;
    }

    public StatusMessage(bool success, Message message)
    {
        Success = success;
        Message = message;
    }
    
    public StatusMessage(bool success, Message message, Player winner, int roundNumber)
    {
        Success = success;
        Message = message;
        Winner = winner;
        RoundNumber = roundNumber;
    }
}
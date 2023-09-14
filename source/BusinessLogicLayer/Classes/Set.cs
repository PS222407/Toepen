using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Set
{
    private const int MaxRounds = 4;

    public List<Round> Rounds { get; private set; }

    public GameStates State { get; private set; }

    public List<Player> Players { get; private set; }

    private Player _playerWhoKnocked;

    public Set(List<Player> players)
    {
        Players = players;
        State = GameStates.ActiveLaundryTimer;
    }

    public StatusMessage PlayerCallsDirtyLaundry(Player player)
    {
        if (State != GameStates.ActiveLaundryTimer)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.CantPerformActionDuringThisGameState
            };
        }

        if (player.HasCalledDirtyLaundry)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.AlreadyCalledLaundry,
            };
        }

        player.CalledDirtyLaundry();

        return new StatusMessage
        {
            Success = true,
        };
    }

    public bool PlayerCallsWhiteLaundry(Player player)
    {
        if (State != GameStates.ActiveLaundryTimer)
        {
            return false;
        }

        player.CalledWhiteLaundry();

        return true;
    }

    public bool StopLaundryTimer()
    {
        if (State != GameStates.ActiveLaundryTimer)
        {
            return false;
        }

        State = GameStates.ActiveTurnLaundryTimer;

        return true;
    }

    public StatusMessage TurnsLaundry(Player turner, Player victim)
    {
        if (State != GameStates.ActiveTurnLaundryTimer)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.CantPerformActionDuringThisGameState
            };
        }
        
        //TODO: when give new cards set LaundryHasBeenTurned back to false
        //TODO for some reason this always true idk why
        if (victim.LaundryHasBeenTurned)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.AlreadyTurnedLaundry,
            };
        }

        if (victim.HasCalledDirtyLaundry)
        {
            if (victim.TurnsAndChecksDirtyLaundry())
            {
                turner.AddPenaltyPoints(1);
                return new StatusMessage
                {
                    Success = true,
                    Message = Messages.PlayerDidNotBluff,
                };
            } 

            if (Settings.LaundryOpenCards)
            {
                victim.AddPenaltyPoints(1);
            }
            else
            {
                // TODO:
                // victim.PlayWithOpenCards(1);
            }

            return new StatusMessage
            {
                Success = true,
                Message = Messages.PlayerDidBluff,
            };
        }

        if (victim.HasCalledWhiteLaundry)
        {
            if (victim.TurnsAndChecksWhiteLaundry())
            {
                turner.AddPenaltyPoints(1);
                return new StatusMessage
                {
                    Success = true,
                    Message = Messages.PlayerDidNotBluff,
                };
            }

            if (Settings.LaundryOpenCards)
            {
                victim.AddPenaltyPoints(1);
            }
            else
            {
                // TODO:
                // victim.PlayWithOpenCards(1);
            }

            return new StatusMessage
            {
                Success = true,
                Message = Messages.PlayerDidBluff,
            };
        }

        return new StatusMessage
        {
            Success = false,
            Message = Messages.PlayerHasNotCalledForLaundry,
        };
    }
}
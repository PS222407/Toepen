using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Set
{
    private const int MaxRounds = 4;

    public List<Round> Rounds { get; private set; }

    public Round CurrentRound { get; private set; }

    public GameStates State { get; private set; }

    public List<Player> Players { get; private set; }

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

        if (player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry)
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

    public StatusMessage PlayerCallsWhiteLaundry(Player player)
    {
        if (State != GameStates.ActiveLaundryTimer)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.CantPerformActionDuringThisGameState,
            };
        }
        
        if (player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.AlreadyCalledLaundry,
            };
        }

        player.CalledWhiteLaundry();

        return new StatusMessage
        {
            Success = true,
        };
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
                victim.MustPlayWithOpenCards();
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
                victim.MustPlayWithOpenCards();
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

    public bool StopLaundryTurnTimerAndStartLaundryTimer()
    {
        if (State != GameStates.ActiveTurnLaundryTimer)
        {
            return false;
        }

        State = GameStates.ActiveLaundryTimer;

        return true;
    }
    
    public bool StopLaundryTurnTimerAndStartRound()
    {
        if (State != GameStates.ActiveTurnLaundryTimer)
        {
            return false;
        }
        
        StartNewRound();

        return true;
    }

    private void StartNewRound()
    {
        State = GameStates.ActiveRound;
        CurrentRound = new Round(Players);
        Rounds.Add(CurrentRound);
    }
}
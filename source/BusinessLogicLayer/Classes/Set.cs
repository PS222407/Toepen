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

    public bool PlayerCallsDirtyLaundry(Player player)
    {
        if (State != GameStates.ActiveLaundryTimer)
        {
            return false;
        }

        player.CalledDirtyLaundry();

        return true;
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

    public bool LaundryTimeIsUp()
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

        if (victim.HasCalledDirtyLaundry)
        {
            if (victim.HasDirtyLaundry())
            {
                turner.AddPenaltyPoints(1);
                return new StatusMessage
                {
                    Success = true,
                    Message = Messages.PlayerDidNotBluff,
                };
            }

            victim.AddPenaltyPoints(1);
            return new StatusMessage
            {
                Success = true,
                Message = Messages.PlayerDidBluff,
            };
        }

        if (victim.HasCalledWhiteLaundry)
        {
            if (victim.HasWhiteLaundry())
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
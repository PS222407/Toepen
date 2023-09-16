using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Set
{
    private const int MaxRounds = 4;

    public List<Round> Rounds { get; private set; } = new();

    public Round CurrentRound { get; private set; }

    public GameState State { get; private set; }

    public List<Player> Players { get; private set; }

    public int PenaltyPoints { get; private set; } = 1;

    public Player SetWinner { get; private set; }

    public Set(List<Player> players)
    {
        Players = players;
        State = GameState.ActiveLaundryTimer;
    }

    public StatusMessage PlayerCallsDirtyLaundry(Player player)
    {
        if (State != GameState.ActiveLaundryTimer)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        if (player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry)
        {
            return new StatusMessage(false, Message.AlreadyCalledLaundry);
        }

        player.CalledDirtyLaundry();

        return new StatusMessage(true);
    }

    public StatusMessage PlayerCallsWhiteLaundry(Player player)
    {
        if (State != GameState.ActiveLaundryTimer)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        if (player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry)
        {
            return new StatusMessage(false, Message.AlreadyCalledLaundry);
        }

        player.CalledWhiteLaundry();

        return new StatusMessage(true);
    }

    public bool StopLaundryTimer()
    {
        if (State != GameState.ActiveLaundryTimer)
        {
            return false;
        }

        State = GameState.ActiveTurnLaundryTimer;

        return true;
    }

    public StatusMessage TurnsLaundry(Player turner, Player victim)
    {
        if (State != GameState.ActiveTurnLaundryTimer)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        if (victim.LaundryHasBeenTurned)
        {
            return new StatusMessage(false, Message.AlreadyTurnedLaundry);
        }

        if (victim.HasCalledDirtyLaundry)
        {
            if (victim.TurnsAndChecksDirtyLaundry())
            {
                turner.AddPenaltyPoints(1);
                return new StatusMessage(true, Message.PlayerDidNotBluff);
            }

            if (Settings.LaundryOpenCards)
            {
                victim.AddPenaltyPoints(1);
            }
            else
            {
                victim.MustPlayWithOpenCards();
            }

            return new StatusMessage(true, Message.PlayerDidBluff);
        }

        if (victim.HasCalledWhiteLaundry)
        {
            if (victim.TurnsAndChecksWhiteLaundry())
            {
                turner.AddPenaltyPoints(1);
                return new StatusMessage(true, Message.PlayerDidNotBluff);
            }

            if (Settings.LaundryOpenCards)
            {
                victim.AddPenaltyPoints(1);
            }
            else
            {
                victim.MustPlayWithOpenCards();
            }

            return new StatusMessage(true, Message.PlayerDidBluff);
        }

        return new StatusMessage(false, Message.PlayerHasNotCalledForLaundry);
    }

    public bool StopLaundryTurnTimerAndStartLaundryTimer()
    {
        if (State != GameState.ActiveTurnLaundryTimer)
        {
            return false;
        }

        State = GameState.ActiveLaundryTimer;

        return true;
    }

    public bool StopLaundryTurnTimerAndStartRound()
    {
        if (State != GameState.ActiveTurnLaundryTimer)
        {
            return false;
        }

        StartNewRound(true, false);

        return true;
    }

    private void StartNewRound(bool noWinner, bool roundWinner)
    {
        if (noWinner)
        {
            CurrentRound = new Round(Players);
            State = GameState.ActiveRound;
            Rounds.Add(CurrentRound);
        }
        else if (roundWinner)
        {
            CurrentRound = new Round(Players, CurrentRound.WinnerStatus!.Winner, PenaltyPoints);
            State = GameState.ActiveRound;
            Rounds.Add(CurrentRound);
        }
    }

    public StatusMessage PlayCard(Player player, Card card)
    {
        StatusMessage statusMessage = CurrentRound.PlayCard(player, card);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            int roundId = CurrentRound.Id;
            HandleWinner();
            return new StatusMessage(true, winnerStatus.WinnerOfSet || Rounds.Count == MaxRounds ? Message.APlayerHasWonSet : Message.APlayerHasWonRound, winnerStatus.Winner, roundId);
        }

        return statusMessage;
    }

    public StatusMessage Knock(Player player)
    {
        return CurrentRound.Knock(player);
    }

    public StatusMessage Fold(Player player)
    {
        StatusMessage statusMessage = CurrentRound.Fold(player);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            int roundId = CurrentRound.Id;
            HandleWinner();
            return new StatusMessage(true, winnerStatus.WinnerOfSet || Rounds.Count == MaxRounds ? Message.APlayerHasWonSet : Message.APlayerHasWonRound, winnerStatus.Winner, roundId);
        }

        PenaltyPoints = CurrentRound.PenaltyPoints;

        return statusMessage;
    }

    public StatusMessage Check(Player player)
    {
        StatusMessage statusMessage = CurrentRound.Check(player);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            int roundId = CurrentRound.Id;
            HandleWinner();
            return new StatusMessage(true, winnerStatus.WinnerOfSet || Rounds.Count == MaxRounds ? Message.APlayerHasWonSet : Message.APlayerHasWonRound, winnerStatus.Winner, roundId);
        }

        PenaltyPoints = CurrentRound.PenaltyPoints;

        return statusMessage;
    }

    private void HandleWinner()
    {
        bool noWinner = CurrentRound.WinnerStatus?.Winner == null;
        bool roundWinner = CurrentRound.WinnerStatus?.Winner != null && !CurrentRound.WinnerStatus.WinnerOfSet;
        if (noWinner)
        {
            StartNewRound(noWinner, roundWinner);
        }
        else if (roundWinner && Rounds.Count < 4)
        {
            StartNewRound(noWinner, roundWinner);
        }
        else
        {
            State = GameState.SetHasBeenWon;
            SetWinner = CurrentRound.WinnerStatus!.Winner;
        }
    }
}
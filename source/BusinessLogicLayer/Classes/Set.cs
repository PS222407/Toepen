using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Set
{
    public List<Round> Rounds { get; private set; } = new();

    public Round CurrentRound { get; private set; }

    public GameState State { get; private set; }

    public List<Player> Players { get; private set; }

    public int PenaltyPoints { get; private set; } = 1;

    public Player WinnerOfSet { get; private set; }

    private readonly Player? _previousSetWinner;

    private List<Card> _deck = new();

    private Player? _lastPlayerWhoKnocked;

    public Set(List<Player> players, Player? previousSetWinner = null)
    {
        if (previousSetWinner != null)
        {
            _previousSetWinner = previousSetWinner;
        }

        Players = players;
        foreach (Player player in Players)
        {
            player.ResetVariablesForNewSet();
        }

        InitializeDeck();
        DealCardsToPlayers();

        State = GameState.ActiveLaundryTimer;
    }

    private void InitializeDeck()
    {
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Value value in Enum.GetValues(typeof(Value)))
            {
                Card card = new Card(suit, value);
                _deck.Add(card);
            }
        }
    }

    private void ShuffleDeck()
    {
        Random rnd = new Random();
        List<Card> shuffledDeck = _deck.OrderBy(x => rnd.Next()).ToList();
        _deck = shuffledDeck;
    }

    private void DealCardsToPlayers()
    {
        ShuffleDeck();

        foreach (Player player in Players)
        {
            DealCardsToPlayer(player);
        }
    }

    private void DealCardsToPlayer(Player player)
    {
        for (int i = 0; i < Settings.AmountStartCardsPlayer; i++)
        {
            Card nextCard = _deck.First();
            player.DealCard(nextCard);
            _deck.Remove(nextCard);
        }
    }

    private void PlayerHandToDeck(Player player)
    {
        foreach (Card card in new List<Card>(player.Hand))
        {
            _deck.Add(card);
            player.RemoveCardFromHand(card);
        }

        ShuffleDeck();
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
                PlayerHandToDeck(victim);
                DealCardsToPlayer(victim);
                return new StatusMessage(true, Message.PlayerDidNotBluff);
            }

            if (Settings.LaundryOpenCards)
            {
                victim.AddPenaltyPoints(1);
                victim.MustPlayWithOpenCards();
            }
            else
            {
                victim.AddPenaltyPoints(1);
            }

            return new StatusMessage(true, Message.PlayerDidBluff);
        }

        if (victim.HasCalledWhiteLaundry)
        {
            if (victim.TurnsAndChecksWhiteLaundry())
            {
                turner.AddPenaltyPoints(1);
                PlayerHandToDeck(victim);
                DealCardsToPlayer(victim);
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

        foreach (Player player in Players)
        {
            if ((player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry) && !player.LaundryHasBeenTurned)
            {
                PlayerHandToDeck(player);
                DealCardsToPlayer(player);
                player.ResetLaundryVariables();
            }
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

        foreach (Player player in Players)
        {
            if ((player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry) && !player.LaundryHasBeenTurned)
            {
                PlayerHandToDeck(player);
                DealCardsToPlayer(player);
            }

            player.ResetLaundryVariables();
        }

        if (_previousSetWinner != null)
        {
            StartNewRound(false, true, _previousSetWinner, true);
        }
        else
        {
            StartNewRound(true, true);
        }

        return true;
    }

    private void StartNewRound(bool noWinner, bool roundWinner, Player? previousSetWinner = null, bool fromNewSet = false)
    {
        if (noWinner)
        {
            CurrentRound = new Round(Players);
            State = GameState.ActiveRound;
            Rounds.Add(CurrentRound);
        }
        else if (roundWinner)
        {
            Player previousWinner = previousSetWinner ?? CurrentRound.WinnerStatus!.Winner;
            CurrentRound = new Round(Players, previousWinner, PenaltyPoints, fromNewSet);
            State = GameState.ActiveRound;
            Rounds.Add(CurrentRound);
        }
    }

    public StatusMessage PlayCard(Player player, Card card)
    {
        if (State != GameState.ActiveRound)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        StatusMessage statusMessage = CurrentRound.PlayCard(player, card);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            int roundNumber = Rounds.Count;
            Message message = winnerStatus.WinnerOfSet || Rounds.Count == Settings.MaxRounds ? Message.APlayerHasWonSet : Message.APlayerHasWonRound;
            HandleWinner();
            return new StatusMessage(true, message, winnerStatus.Winner, roundNumber);
        }

        return statusMessage;
    }

    public StatusMessage Knock(Player player)
    {
        if (_lastPlayerWhoKnocked == player)
        {
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);
        }

        StatusMessage statusMessage = CurrentRound.Knock(player);
        if (statusMessage.Success)
        {
            _lastPlayerWhoKnocked = player;
        }

        return statusMessage;
    }

    public StatusMessage Fold(Player player)
    {
        StatusMessage statusMessage = CurrentRound.Fold(player);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            int roundNumber = Rounds.Count;
            HandleWinner();
            return new StatusMessage(true, winnerStatus.WinnerOfSet || Rounds.Count == Settings.MaxRounds ? Message.APlayerHasWonSet : Message.APlayerHasWonRound, winnerStatus.Winner, roundNumber);
        }

        PenaltyPoints = CurrentRound.PenaltyPoints;

        return statusMessage;
    }

    public StatusMessage Check(Player player)
    {
        if (State != GameState.ActiveRound)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        StatusMessage statusMessage = CurrentRound.Check(player);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            int roundNumber = Rounds.Count;
            Message message = winnerStatus.WinnerOfSet || Rounds.Count == Settings.MaxRounds ? Message.APlayerHasWonSet : Message.APlayerHasWonRound;
            HandleWinner();
            return new StatusMessage(true, message, winnerStatus.Winner, roundNumber);
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
            WinnerOfSet = CurrentRound.WinnerStatus!.Winner;

            foreach (Player player in Players.Where(p => !p.IsOutOfGame()))
            {
                if (player != WinnerOfSet)
                {
                    player.AddPenaltyPoints(PenaltyPoints);
                }
            }
        }
    }
}
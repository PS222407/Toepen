using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Helpers;

namespace Toepen_20_BusinessLogicLayer.Models;

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

        player.CallsDirtyLaundry();

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

        player.CallsWhiteLaundry();

        return new StatusMessage(true);
    }

    public bool BlockLaundryCalls()
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

    public bool BlockLaundryTurnCallsAndWaitForLaundryCalls()
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

    public bool BlockLaundryTurnCallsAndStartRound()
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

    public WinnerStatus? PlayCard(Player player, Card card)
    {
        CurrentRound.PlayCard(player, card);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            winnerStatus.WinnerOfSet = winnerStatus.WinnerOfSet || Rounds.Count == Settings.MaxRounds;
            winnerStatus.RoundNumber = Rounds.Count;
            
            HandleWinner();
            
            return winnerStatus;
        }

        return null;
    }

    public void Knock(Player player)
    {
        if (_lastPlayerWhoKnocked == player)
        {
            throw new CantPerformToSelfException();
        }

        StatusMessage statusMessage = CurrentRound.Knock(player);
        if (statusMessage.Success)
        {
            _lastPlayerWhoKnocked = player;
        }
    }

    public WinnerStatus? Fold(Player player)
    {
        CurrentRound.Fold(player);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            winnerStatus.WinnerOfSet = winnerStatus.WinnerOfSet || Rounds.Count == Settings.MaxRounds;
            winnerStatus.RoundNumber = Rounds.Count;
            
            HandleWinner();
            
            return winnerStatus;
        }

        PenaltyPoints = CurrentRound.PenaltyPoints;

        return null;
    }

    public WinnerStatus? Check(Player player)
    {
        CurrentRound.Check(player);
        if (CurrentRound.WinnerStatus?.Winner != null)
        {
            WinnerStatus winnerStatus = CurrentRound.WinnerStatus;
            winnerStatus.WinnerOfSet = winnerStatus.WinnerOfSet || Rounds.Count == Settings.MaxRounds;
            winnerStatus.RoundNumber = Rounds.Count;

            HandleWinner();
            
            return winnerStatus;
        }

        PenaltyPoints = CurrentRound.PenaltyPoints;

        return null;
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
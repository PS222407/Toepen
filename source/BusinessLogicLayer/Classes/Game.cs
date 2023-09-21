using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Game
{
    private const int MinAmountOfPlayer = 2;

    private const int MaxAmountOfPlayers = 6;

    private bool _gameHasStarted;

    public GameState GameState { get; }

    private List<Player> _players = new();
    public IReadOnlyList<Player> Players => _players;

    private List<Set> _sets = new();
    public IReadOnlyList<Set> Sets => _sets;
    
    public Set? CurrentSet { get; private set; }

    public bool AddPlayer(Player player)
    {
        if (_gameHasStarted)
        {
            return false;
        }

        if (Players.Count < MaxAmountOfPlayers)
        {
            _players.Add(player);
            return true;
        }

        return false;
    }

    // System input actions
    public StatusMessage Start()
    {
        if (_gameHasStarted)
        {
            return new StatusMessage(false, Message.GameAlreadyStarted);
        }

        if (Players.Count < MinAmountOfPlayer)
        {
            return new StatusMessage(false, Message.MinimumPlayersNotReached);
        }

        _gameHasStarted = true;

        StartNewSet();

        return new StatusMessage(true);
    }

    // Player input actions
    public StatusMessage DirtyLaundry(int playerId)
    {
        Player? player = _players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Message.PlayerNotFound);
        }

        return CurrentSet!.PlayerCallsDirtyLaundry(player);
    }

    public StatusMessage WhiteLaundry(int playerId)
    {
        Player? player = _players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Message.PlayerNotFound);
        }

        return CurrentSet!.PlayerCallsWhiteLaundry(player);
    }

    public StatusMessage TurnsLaundry(int playerId, int victimId)
    {
        if (playerId == victimId)
        {
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);
        }

        Player? player = _players.Find(p => p.Id == playerId);
        Player? victim = _players.Find(p => p.Id == victimId);

        if (player == null || victim == null)
        {
            return new StatusMessage(false, Message.PlayerNotFound);
        }

        StatusMessage statusMessage = CurrentSet!.TurnsLaundry(player, victim);

        return statusMessage;
    }

    public bool StopLaundryTurnTimerAndStartLaundryTimer()
    {
        return CurrentSet!.StopLaundryTurnTimerAndStartLaundryTimer();
    }

    public bool StopLaundryTurnTimerAndStartRound()
    {
        return CurrentSet!.StopLaundryTurnTimerAndStartRound();
    }

    public bool StopLaundryTimer()
    {
        return CurrentSet!.StopLaundryTimer();
    }

    public StatusMessage Knock(int playerId)
    {
        Player? player = _players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Message.PlayerNotFound);
        }

        return CurrentSet!.Knock(player);
    }

    public StatusMessage Check(int playerId)
    {
        Player? player = _players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Message.PlayerNotFound);
        }

        return CurrentSet!.Check(player);
    }

    public StatusMessage Fold(int playerId)
    {
        Player? player = _players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Message.PlayerNotFound);
        }

        return CurrentSet!.Fold(player);
    }

    public StatusMessage PlayCard(int playerId, string value, string suit)
    {
        Player? player = _players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Message.PlayerNotFound);
        }

        Value? cardValue = TransformValue(value);
        Suit? cardSuit = TransformSuit(suit);
        if (cardValue == null || cardSuit == null)
        {
            return new StatusMessage(false, Message.CardNotFound);
        }

        StatusMessage statusMessage = CurrentSet!.PlayCard(player, new Card(cardSuit.Value, cardValue.Value));
        if (!statusMessage.Success)
        {
            return statusMessage;
        }

        if (statusMessage.Message == Message.APlayerHasWonSet)
        {
            bool gameHasAWinner = Players.Where(p => !p.IsOutOfGame()).ToList().Count == 1;
            if (gameHasAWinner)
            {
                return new StatusMessage(true, Message.APlayerHasWonGame);
            }

            StartNewSet();
        }

        return statusMessage;
    }

    private Player? GetWinner()
    {
        bool gameHasAWinner = Players.Where(p => !p.IsOutOfGame()).ToList().Count == 1;
        if (gameHasAWinner)
        {
            return Players.First(p => !p.IsOutOfGame());
        }

        return null;
    }

    private void StartNewSet()
    {
        Player? winnerOfSet = _sets.LastOrDefault()?.WinnerOfSet;
        CurrentSet = new Set(_players.Where(p => !p.IsDead()).ToList(), winnerOfSet);
        _sets.Add(CurrentSet);
    }

    private Value? TransformValue(string value)
    {
        switch (value.ToUpper())
        {
            case "J":
                return Value.Jack;
            case "Q":
                return Value.Queen;
            case "K":
                return Value.King;
            case "A":
                return Value.Ace;
            case "7":
                return Value.Seven;
            case "8":
                return Value.Eight;
            case "9":
                return Value.Nine;
            case "10":
                return Value.Ten;
        }

        return null;
    }

    private Suit? TransformSuit(string suit)
    {
        switch (suit.ToUpper())
        {
            case "S":
                return Suit.Spades;
            case "D":
                return Suit.Diamonds;
            case "C":
                return Suit.Clubs;
            case "H":
                return Suit.Hearts;
        }

        return null;
    }
}
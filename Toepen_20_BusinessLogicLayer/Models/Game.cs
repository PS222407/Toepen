using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.States;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Game
{
    public string RoomId { get; }

    public const int MinAmountOfPlayer = 2;

    public const int MaxAmountOfPlayers = 6;

    public GameState GameState { get; }

    public List<Player> Players { get; set; } = new();

    private List<Set> _sets = new();
    public IReadOnlyList<Set> Sets => _sets;

    public Set? CurrentSet { get; private set; }

    public IState State { get; set; } = new Initialized();

    public Game()
    {
        
    }

    public Game(string roomId)
    {
        RoomId = roomId;
    }

    public void AddPlayer(Player player)
    {
        State.AddPlayer(this, player);
    }

    // System input actions
    public void Start()
    {
        State.Start(this);
        StartNewSet();
    }

    // Player input actions
    public void PlayerCallsDirtyLaundry(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerCallsDirtyLaundry(this, player);
    }

    public void PlayerCallsWhiteLaundry(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerCallsWhiteLaundry(this, player);
    }

    public void PlayerTurnsLaundry(int playerId, int victimId)
    {
        if (playerId == victimId)
        {
            throw new CantPerformToSelfException();
        }

        Player? player = Players.Find(p => p.Id == playerId);
        Player? victim = Players.Find(p => p.Id == victimId);

        if (player == null || victim == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerTurnsLaundry(this, player, victim);
    }

    public void BlockLaundryTurnCallsAndWaitForLaundryCalls()
    {
        State.BlockLaundryTurnCallsAndWaitForLaundryCalls(this);
    }

    public void BlockLaundryTurnCallsAndStartRound()
    {
        State.BlockLaundryTurnCallsAndStartRound(this);
    }

    public void BlockLaundryCalls()
    {
        State.BlockLaundryCalls(this);
    }

    public void Knock(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerKnocks(this, player);
    }

    public void Check(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }


        State.PlayerChecks(this, player);
    }

    public void Fold(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }


        State.PlayerFolds(this, player);
    }

    public void PlayerPlaysCard(int playerId, string value, string suit)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        Value? cardValue = TransformValue(value);
        Suit? cardSuit = TransformSuit(suit);
        if (cardValue == null || cardSuit == null)
        {
            throw new CardNotFoundException();
        }

        State.PlayerPlaysCard(this, player, new Card(cardSuit.Value, cardValue.Value));
    }

    public Player? GetWinner()
    {
        return Players.Where(p => !p.IsOutOfGame()).ToList().Count == 1 ? Players.First(p => !p.IsOutOfGame()) : null;
    }

    public void StartNewSet()
    {
        Player? winnerOfSet = _sets.LastOrDefault()?.WinnerOfSet;
        CurrentSet = new Set(Players.Where(p => !p.IsDead()).ToList(), winnerOfSet);
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
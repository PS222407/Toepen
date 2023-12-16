using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.States;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Game
{
    public string RoomCode { get; }

    public const int MinAmountOfPlayer = 2;

    public const int MaxAmountOfPlayers = 6;

    public GameState GameState { get; }

    public List<Player> Players { get; set; } = new();

    private List<Set> _sets = new();

    public IReadOnlyList<Set> Sets => _sets;

    public Set? CurrentSet { get; private set; }

    public IState State { get; set; } = new Initialized();

    public Game(string roomCode)
    {
        RoomCode = roomCode;
    }

    /// <exception cref="InvalidStateException"></exception>
    public TimerInfo? TimerCallback()
    {
        try
        {
            TimerInfo laundryTimerInfo = State.LaundryTimerCallback(this);
            return laundryTimerInfo;
        }
        catch (InvalidStateException)
        {
        }

        try
        {
            TimerInfo laundryTurnTimerInfo = State.LaundryTurnTimerCallback(this);
            return laundryTurnTimerInfo;
        }
        catch (InvalidStateException)
        {
        }

        return null;
    }

    public Player? FindPlayerByConnectionId(string connectionId)
    {
        return Players.Find(p => p.ConnectionId == connectionId);
    }

    public Player? FindPlayerById(int id)
    {
        return Players.Find(p => p.Id == id);
    }

    public void AddPlayer(Player player)
    {
        if (Players.Count == 0)
        {
            player.SetAsHost();
        }

        State.AddPlayer(this, player);
    }

    public void RemovePlayer(Player victim)
    {
        State.RemovePlayer(this, victim);
    }

    public bool AnyPlayerCalledLaundry()
    {
        return Players.Any(p => p.HasCalledDirtyLaundry || p.HasCalledWhiteLaundry);
    }

    public Player? GetActivePlayer()
    {
        return CurrentSet?.CurrentRound?.ActivePlayer;
    }

    public Player? GetPlayerWhoKnocked()
    {
        return CurrentSet?.GetLastKnockedPlayer();
    }

    #region Player input actions

    /// <exception cref="AlreadyStartedException"></exception>
    /// <exception cref="NotEnoughPlayersException"></exception>
    public void Start()
    {
        State.Start(this);
    }

    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsDirtyLaundry(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerCallsDirtyLaundry(this, player);
    }

    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsWhiteLaundry(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerCallsWhiteLaundry(this, player);
    }

    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsNoLaundry(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerCallsNoLaundry(this, player);
    }

    /// <exception cref="CantPerformToSelfException"></exception>
    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="PlayerHasNotCalledForLaundryException"></exception>
    /// <exception cref="AlreadyTurnedException"></exception>
    /// <exception cref="InvalidStateException"></exception>
    public Message PlayerTurnsLaundry(int playerId, int victimId)
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

        return State.PlayerTurnsLaundry(this, player, victim);
    }

    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
    /// <exception cref="CantPerformToSelfException"></exception>
    /// <exception cref="NotPlayersTurnException"></exception>
    /// <exception cref="PlayerIsAllInException"></exception>
    public void PlayerKnocks(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerKnocks(this, player);
    }

    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
    public void PlayerChecks(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerChecks(this, player);
    }

    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
    public void PlayerFolds(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerFolds(this, player);
    }

    /// <exception cref="InvalidStateException"></exception>
    /// <exception cref="NotPlayersTurnException"></exception>
    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="CardDoesNotMatchSuitsException"></exception>
    /// <exception cref="CardNotFoundException"></exception>
    public void PlayerPlaysCard(int playerId, Card card)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            throw new PlayerNotFoundException();
        }

        State.PlayerPlaysCard(this, player, new Card(card.Suit, card.Value));
    }

    #endregion

    /// <exception cref="InvalidStateException"></exception>
    public void BlockLaundryTurnCalls()
    {
        State.BlockLaundryTurnCalls(this);
    }

    /// <exception cref="InvalidStateException"></exception>
    public void BlockLaundryCalls()
    {
        State.BlockLaundryCalls(this);
    }

    public Player? GetWinner()
    {
        return Players.Where(p => !p.IsDead()).ToList().Count == 1 ? Players.First(p => !p.IsDead()) : null;
    }

    public void StartNewSet()
    {
        Player? winnerOfSet = _sets.LastOrDefault()?.WinnerOfSet;

        Players.Where(p => p.IsDead()).ToList().ForEach(p => p.ResetVariablesForNewSet());
        
        CurrentSet = new Set(Players.Where(p => !p.IsDead()).ToList(), winnerOfSet);
        _sets.Add(CurrentSet);
    }

    public void PlayerCallsMoveOnToNextSet(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId) ?? throw new PlayerNotFoundException();
        State.PlayerMovesOnToNextSet(this, player);
    }
}
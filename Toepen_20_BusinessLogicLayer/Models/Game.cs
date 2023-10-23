using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.States;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Game
{
    public string RoomId { get; }

    public const int MinAmountOfPlayer = 2;

    public const int MaxAmountOfPlayers = 6;

    private DateTime _endTimeCountDown;

    public GameState GameState { get; }

    public List<Player> Players { get; set; } = new();

    private List<Set> _sets = new();

    public IReadOnlyList<Set> Sets => _sets;

    public Set? CurrentSet { get; private set; }

    public IState State { get; set; } = new Initialized();

    public Game(string roomId)
    {
        RoomId = roomId;
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
        State.AddPlayer(this, player);
    }

    public Player? GetActivePlayer()
    {
        return CurrentSet?.CurrentRound?.ActivePlayer;
    }

    public Player? GetPlayerWhoKnocked()
    {
        return CurrentSet?.CurrentRound?.PlayerWhoKnocked;
    }

    #region Player input actions

    /// <exception cref="AlreadyStartedException"></exception>
    /// <exception cref="NotEnoughPlayersException"></exception>
    public void Start()
    {
        State.Start(this);
        StartCountDown();
        StartNewSet();
    }
    
    private void StartCountDown()
    {
        int timeCountdownInSeconds = 20;
        
        DateTime startTime = DateTime.Now;
        _endTimeCountDown = startTime.AddSeconds(timeCountdownInSeconds);
    }

    public int GetTimeLeftCountdown()
    {
        if (_endTimeCountDown > DateTime.Now)
        {
            TimeSpan timeLeft = _endTimeCountDown - DateTime.Now;
            int secondsLeft = (int)Math.Floor(timeLeft.TotalSeconds);
            return secondsLeft;
        }

        return 0;   
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

    /// <exception cref="CantPerformToSelfException"></exception>
    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
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

    /// <exception cref="PlayerNotFoundException"></exception>
    /// <exception cref="InvalidStateException"></exception>
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
    public void BlockLaundryTurnCallsAndWaitForLaundryCalls()
    {
        State.BlockLaundryTurnCallsAndWaitForLaundryCalls(this);
    }

    /// <exception cref="InvalidStateException"></exception>
    public void BlockLaundryTurnCallsAndStartRound()
    {
        State.BlockLaundryTurnCallsAndStartRound(this);
    }

    /// <exception cref="InvalidStateException"></exception>
    public void BlockLaundryCalls()
    {
        State.BlockLaundryCalls(this);
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
}
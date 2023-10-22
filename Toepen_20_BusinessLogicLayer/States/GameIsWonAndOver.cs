using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class GameIsWonAndOver : IState
{
    public void AddPlayer(Game game, Player player)
    {
        throw new AlreadyStartedException();
    }

    public void Start(Game game)
    {
        throw new AlreadyStartedException();
    }

    public void PlayerCallsDirtyLaundry(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerCallsWhiteLaundry(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerTurnsLaundry(Game game, Player player, Player victim)
    {
        throw new InvalidStateException();
    }

    public void BlockLaundryTurnCallsAndWaitForLaundryCalls(Game game)
    {
        throw new InvalidStateException();
    }

    public void BlockLaundryTurnCallsAndStartRound(Game game)
    {
        throw new InvalidStateException();
    }

    public void BlockLaundryCalls(Game game)
    {
        throw new InvalidStateException();
    }

    public void PlayerKnocks(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerChecks(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerFolds(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerPlaysCard(Game game, Player player, Card card)
    {
        throw new InvalidStateException();
    }

    public Player GetWinner(Game game)
    {
        throw new NotImplementedException();
    }

    public void StartNewSet(Game game)
    {
        throw new InvalidStateException();
    }
}
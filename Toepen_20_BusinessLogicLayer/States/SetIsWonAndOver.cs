using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class SetIsWonAndOver : IState
{
    public void AddPlayer(Game game, Player player)
    {
        throw new AlreadyStartedException();
    }
    
    public void RemovePlayer(Game game, Player player)
    {
        player.Disconnect();
        if (game.GetWinner() != null)
        {
            game.State = new GameIsWonAndOver();
        }
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

    public void PlayerCallsNoLaundry(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public Message PlayerTurnsLaundry(Game game, Player player, Player victim)
    {
        throw new InvalidStateException();
    }

    public void BlockLaundryTurnCalls(Game game)
    {
        throw new InvalidStateException();
    }

    public void BlockLaundryCalls(Game game)
    {
        throw new InvalidStateException();
    }

    public TimerInfo LaundryTimerCallback(Game game)
    {
        throw new InvalidStateException();
    }

    public TimerInfo LaundryTurnTimerCallback(Game game)
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

    public void PlayerMovesOnToNextSet(Game game, Player player)
    {
        if (player.IsDead())
        {
            throw new PlayerIsDeadException();
        }
        
        game.CurrentSet!.PlayerCallsMoveOnToNextSet(player);

        if (game.Players.Where(p => !p.IsDead()).All(p => p.HasCalledMoveOnToNextSet))
        {
            game.StartNewSet();
            game.State = new WaitingForLaundryCalls();
        }
    }
}
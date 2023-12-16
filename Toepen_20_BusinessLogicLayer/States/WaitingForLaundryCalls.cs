using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class WaitingForLaundryCalls : IState
{
    public void AddPlayer(Game game, Player player)
    {
        throw new AlreadyStartedException();
    }
    
    public void RemovePlayer(Game game, Player victim)
    {
        throw new AlreadyStartedException();
    }

    public void Start(Game game)
    {
        throw new AlreadyStartedException();
    }

    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsDirtyLaundry(Game game, Player player)
    {
        game.CurrentSet!.PlayerCallsDirtyLaundry(player);
    }

    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsWhiteLaundry(Game game, Player player)
    {
        game.CurrentSet!.PlayerCallsWhiteLaundry(player);
    }

    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsNoLaundry(Game game, Player player)
    {
        game.CurrentSet!.PlayerCallsNoLaundry(player);
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
        game.CurrentSet!.BlockLaundryCalls();
        if (game.AnyPlayerCalledLaundry())
        {
            game.State = new WaitingForLaundryTurnCalls();
        }
        else
        {
            if (game.CurrentSet.PreviousSetWinner != null)
            {
                game.CurrentSet.StartNewRound(false, true, game.CurrentSet.PreviousSetWinner, true);
            }
            else
            {
                game.CurrentSet.StartNewRound(true, true);
            }

            game.State = new ActiveRound();
        }
    }

    public TimerInfo LaundryTimerCallback(Game game)
    {
        bool done = false;
        TimerInfo? laundryTimerInfo = game.CurrentSet?.GetTimeLeftLaundryTimerInSeconds();

        if (game.Players.All(p => p.HasNoLaundry || p.HasCalledWhiteLaundry || p.HasCalledDirtyLaundry) ||
            (game.State.GetType() == typeof(WaitingForLaundryCalls) && laundryTimerInfo?.Seconds == -1))
        {
            game.State.BlockLaundryCalls(game);
            done = true;
        }

        return new TimerInfo
        {
            Seconds = done ? -1 : (laundryTimerInfo?.Seconds ?? -1),
            First = laundryTimerInfo?.First ?? false,
            Done = done,
        };
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
        throw new InvalidStateException();
    }

    public void StartNewSet(Game game)
    {
        throw new InvalidStateException();
    }

    public void PlayerMovesOnToNextSet(Game game, Player player)
    {
        throw new InvalidStateException();
    }
}
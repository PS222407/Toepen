using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class WaitingForLaundryCalls : IState
{
    public void AddPlayer(Game game, Player player)
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

    public void PlayerTurnsLaundry(Game game, Player player, Player victim)
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
        game.State = new WaitingForLaundryTurnCalls();
    }

    public TimerInfo LaundryTimerCallback(Game game)
    {
        bool done = false;
        TimerInfo? laundryTimerInfo = game.CurrentSet?.GetTimeLeftLaundryTimerInSeconds();

        if (game.State.GetType() == typeof(WaitingForLaundryCalls) && laundryTimerInfo?.Seconds == -1)
        {
            game.State.BlockLaundryCalls(game);
            done = true;
        }

        return new TimerInfo
        {
            Seconds = laundryTimerInfo?.Seconds ?? -1,
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
}
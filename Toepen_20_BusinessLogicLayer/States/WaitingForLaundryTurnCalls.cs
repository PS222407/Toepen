using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class WaitingForLaundryTurnCalls : IState
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

    /// <exception cref="AlreadyTurnedException"></exception>
    public void PlayerTurnsLaundry(Game game, Player player, Player victim)
    {
        game.CurrentSet!.TurnsLaundry(player, victim);
    }

    public void BlockLaundryTurnCalls(Game game)
    {
        game.CurrentSet!.BlockLaundryTurnCalls();
        if (game.CurrentSet.LaundryCardsAreDealt)
        {
            game.CurrentSet!.BlockLaundryTurnCallsAndWaitForLaundryCalls();
            game.State = new WaitingForLaundryCalls();
        }
        else
        {
            game.CurrentSet!.BlockLaundryTurnCallsAndStartRound();
            game.State = new ActiveRound();
        }

        game.CurrentSet.LaundryCardsAreDealt = false;
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
        bool done = false;
        TimerInfo? laundryTurnTimerInfo = game.CurrentSet?.GetTimeLeftLaundryTurnTimerInSeconds();

        if (game.State.GetType() == typeof(WaitingForLaundryTurnCalls) && laundryTurnTimerInfo?.Seconds == -1)
        {
            game.State.BlockLaundryTurnCalls(game);
            done = true;
        }

        return new TimerInfo
        {
            Seconds = laundryTurnTimerInfo?.Seconds ?? -1,
            First = laundryTurnTimerInfo?.First ?? false,
            Done = done,
        };
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
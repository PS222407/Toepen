using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.LogTypes;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class WaitingForLaundryTurnCalls : IState
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

        if (player == game.CurrentSet?.CurrentRound.ActivePlayer)
        {
            game.CurrentSet.CurrentRound.SetNextPlayer();
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

    /// <exception cref="AlreadyTurnedException"></exception>
    /// <exception cref="PlayerHasNotCalledForLaundryException"></exception>
    /// <exception cref="PlayerIsOutOfGameException"></exception>
    public Message PlayerTurnsLaundry(Game game, Player player, Player victim)
    {
        if (player.IsOutOfGame() || victim.IsOutOfGame())
        {
            throw new PlayerIsOutOfGameException();
        }

        Message message = game.CurrentSet!.TurnsLaundry(player, victim);
        game.Logs.Add(new TurnLaundryLog(victim, player, message == Message.PlayerDidBluff));

        return message;
    }

    public void BlockLaundryTurnCalls(Game game)
    {
        if (game.CurrentSet!.LaundryCardsAreDealt || game.CurrentSet.AnyPlayerHasUnturnedLaundry())
        {
            game.CurrentSet!.BlockLaundryTurnCallsAndWaitForLaundryCalls();
            game.State = new WaitingForLaundryCalls();
        }
        else if (game.CurrentSet.Players.Any(p => p.HasPoverty()) && !game.CurrentSet.Players.Where(p => !p.IsOutOfGame()).All(p => p.HasPoverty()))
        {
            game.CurrentSet.PenaltyPoints = 2;
            game.State = new Poverty();
        }
        else
        {
            game.CurrentSet!.StartRound();
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

    public void PlayerMovesOnToNextSet(Game game, Player player)
    {
        throw new InvalidStateException();
    }
}
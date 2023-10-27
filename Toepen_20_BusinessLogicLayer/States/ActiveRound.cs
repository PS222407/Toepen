using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Helpers;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class ActiveRound : IState
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

    /// <exception cref="InvalidStateException"></exception>
    /// <exception cref="CantPerformToSelfException"></exception>
    public void PlayerKnocks(Game game, Player player)
    {
        game.CurrentSet!.Knock(player);
        game.State = new PlayerKnocked();
    }

    public void PlayerChecks(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerFolds(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    /// <exception cref="NotPlayersTurnException"></exception>
    /// <exception cref="CardDoesNotMatchSuitsException"></exception>
    /// <exception cref="CardNotFoundException"></exception>
    public void PlayerPlaysCard(Game game, Player player, Card card)
    {
        WinnerStatus? winnerStatus = game.CurrentSet!.PlayCard(player, card);
        if (winnerStatus == null)
        {
            return;
        }

        if (winnerStatus.WinnerOfSet)
        {
            if (game.GetWinner() != null)
            {
                game.State = new GameIsWonAndOver();
                return;
            }

            game.StartNewSet();
            game.State = new WaitingForLaundryCalls();
        }
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
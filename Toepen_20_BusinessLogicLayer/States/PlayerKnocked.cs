using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Helpers;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class PlayerKnocked : IState
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
        if (player.IsOutOfGame())
        {
            throw new PlayerIsOutOfGameException();
        }
        
        WinnerStatus? winnerStatus = game.CurrentSet!.Check(player);

        if (winnerStatus == null)
        {
            if (game.CurrentSet.CurrentRound.State == GameState.WaitingForCardOrKnock)
            {
                game.State = new ActiveRound();
            }

            return;
        }

        if (winnerStatus.WinnerOfSet)
        {
            if (game.GetWinner() != null)
            {
                return;
            }

            return;
        }

        game.State = new GameIsWonAndOver();
    }

    public void PlayerFolds(Game game, Player player)
    {
        if (player.IsOutOfGame())
        {
            throw new PlayerIsOutOfGameException();
        }
        
        WinnerStatus? winnerStatus = game.CurrentSet!.Fold(player);

        if (winnerStatus == null)
        {
            if (game.CurrentSet.CurrentRound.State == GameState.WaitingForCardOrKnock)
            {
                game.State = new ActiveRound();
            }

            return;
        }

        if (winnerStatus.WinnerOfSet)
        {
            if (game.GetWinner() != null)
            {
                game.State = new GameIsWonAndOver();
                return;
            }

            game.State = new SetIsWonAndOver();
        }
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
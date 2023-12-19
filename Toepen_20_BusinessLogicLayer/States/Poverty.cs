using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class Poverty : IState
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

        if (player == game.CurrentSet?.CurrentRound?.ActivePlayer)
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

        game.CurrentSet!.CheckPoverty(player);
        
        if (game.CurrentSet.Players.Where(p => !p.IsOutOfGame() && !p.HasPoverty()).All(p => p.DecidedToPlayPovertyOrNot))
        {
            game.CurrentSet!.StartRound();
            game.State = new ActiveRound();
        }
    }

    public void PlayerFolds(Game game, Player player)
    {
        if (player.IsOutOfGame())
        {
            throw new PlayerIsOutOfGameException();
        }

        game.CurrentSet!.FoldPoverty(player);

        if (game.CurrentSet.Players.Where(p => !p.IsOutOfGame() && !p.HasPoverty()).All(p => p.DecidedToPlayPovertyOrNot && p.HasFolded) &&
            game.CurrentSet.Players.Count(p => !p.IsOutOfGame() && p.HasPoverty()) == 1)
        {
            Player? gameWinner = game.GetWinner();
            Player? setWinner = game.CurrentSet.GetSetWinner();
            if (gameWinner != null)
            {
                game.State = new GameIsWonAndOver();
            }
            else if (setWinner != null)
            {
                game.CurrentSet.WinnerOfSet = setWinner;
                game.State = new SetIsWonAndOver();
            }
        }
        else if (game.CurrentSet.Players.Where(p => !p.IsOutOfGame() && !p.HasPoverty()).All(p => p.DecidedToPlayPovertyOrNot))
        {
            game.CurrentSet!.StartRound();
            game.State = new ActiveRound();
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
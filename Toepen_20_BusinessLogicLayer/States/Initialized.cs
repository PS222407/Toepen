using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class Initialized : IState
{
    public void AddPlayer(Game game, Player player)
    {
        if (game.Players.Count >= Game.MaxAmountOfPlayers)
        {
            throw new TooManyPlayersException();
        }

        game.Players.Add(player);
    }

    public void Start(Game game)
    {
        if (game.Players.Count < Game.MinAmountOfPlayer)
        {
            throw new NotEnoughPlayersException();
        }

        game.State = new WaitingForLaundryCalls();
    }

    public void PlayerCallsDirtyLaundry(Game game, Player player)
    {
        throw new InvalidOperationException();
    }

    public void PlayerCallsWhiteLaundry(Game game, Player player)
    {
        throw new InvalidOperationException();
    }

    public void PlayerTurnsLaundry(Game game, Player player, Player victim)
    {
        throw new InvalidOperationException();
    }

    public void BlockLaundryTurnCallsAndWaitForLaundryCalls(Game game)
    {
        throw new InvalidOperationException();
    }

    public void BlockLaundryTurnCallsAndStartRound(Game game)
    {
        throw new InvalidOperationException();
    }

    public void BlockLaundryCalls(Game game)
    {
        throw new InvalidOperationException();
    }

    public void PlayerKnocks(Game game, Player player)
    {
        throw new InvalidOperationException();
    }

    public void PlayerChecks(Game game, Player player)
    {
        throw new InvalidOperationException();
    }

    public void PlayerFolds(Game game, Player player)
    {
        throw new InvalidOperationException();
    }

    public void PlayerPlaysCard(Game game, Player player, Card card)
    {
        throw new InvalidOperationException();
    }

    public Player GetWinner(Game game)
    {
        throw new InvalidOperationException();
    }

    public void StartNewSet(Game game)
    {
        throw new InvalidOperationException();
    }
}
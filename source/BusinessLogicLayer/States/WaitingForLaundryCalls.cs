using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.States;

public class WaitingForLaundryCalls : IState
{
    public bool TryAddPlayer(Game game, Player player)
    {
        throw new InvalidOperationException();
    }

    public void Start(Game game)
    {
        throw new InvalidOperationException();
    }

    public void PlayerCallsDirtyLaundry(Game game, Player player)
    {
        game.CurrentSet!.PlayerCallsDirtyLaundry(player);
    }

    public void PlayerCallsWhiteLaundry(Game game, Player player)
    {
        game.CurrentSet!.PlayerCallsWhiteLaundry(player);
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
        game.CurrentSet!.BlockLaundryCalls();
        game.State = new WaitingForTurnLaundryCalls();
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
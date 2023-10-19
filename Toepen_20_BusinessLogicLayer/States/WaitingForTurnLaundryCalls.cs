using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class WaitingForTurnLaundryCalls : IState
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
        throw new InvalidOperationException();
    }

    public void PlayerCallsWhiteLaundry(Game game, Player player)
    {
        throw new InvalidOperationException();
    }

    public void PlayerTurnsLaundry(Game game, Player player, Player victim)
    {
        game.CurrentSet!.TurnsLaundry(player, victim);
    }

    public void BlockLaundryTurnCallsAndWaitForLaundryCalls(Game game)
    {
        game.CurrentSet!.BlockLaundryTurnCallsAndWaitForLaundryCalls();
        game.State = new WaitingForLaundryCalls();
    }

    public void BlockLaundryTurnCallsAndStartRound(Game game)
    {
        game.CurrentSet!.BlockLaundryTurnCallsAndStartRound();
        game.State = new ActiveRound();
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
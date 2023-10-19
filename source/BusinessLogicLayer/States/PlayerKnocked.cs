using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.States;

public class PlayerKnocked : IState
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
                return;
            }
                
            return;
        }
            
        game.State = new GameIsWonAndOver();
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
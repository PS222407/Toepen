using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public interface IState
{
    /// <exception cref="AlreadyStartedException"></exception>>
    public void AddPlayer(Game game, Player player);

    /// <exception cref="AlreadyStartedException"></exception>>
    public void Start(Game game);

    /// <exception cref="InvalidStateException"></exception>>
    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsDirtyLaundry(Game game, Player player);

    /// <exception cref="InvalidStateException"></exception>>
    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void PlayerCallsWhiteLaundry(Game game, Player player);

    /// <exception cref="InvalidStateException"></exception>>
    public void PlayerTurnsLaundry(Game game, Player player, Player victim);

    /// <exception cref="InvalidStateException"></exception>>
    public void BlockLaundryTurnCallsAndWaitForLaundryCalls(Game game);

    /// <exception cref="InvalidStateException"></exception>>
    public void BlockLaundryTurnCallsAndStartRound(Game game);

    /// <exception cref="InvalidStateException"></exception>>
    public void BlockLaundryCalls(Game game);

    /// <exception cref="InvalidStateException"></exception>>
    public void PlayerKnocks(Game game, Player player);

    /// <exception cref="InvalidStateException"></exception>>
    public void PlayerChecks(Game game, Player player);

    /// <exception cref="InvalidStateException"></exception>>
    public void PlayerFolds(Game game, Player player);

    /// <exception cref="InvalidStateException"></exception>
    /// <exception cref="DoesNotMatchSuitException"></exception>
    public void PlayerPlaysCard(Game game, Player player, Card card);

    /// <exception cref="InvalidStateException"></exception>>
    public Player GetWinner(Game game);

    /// <exception cref="InvalidStateException"></exception>>
    public void StartNewSet(Game game);
}
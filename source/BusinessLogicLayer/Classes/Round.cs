using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Round
{
    static int _nextId;

    public int Id { get; }

    public List<Player> Players { get; private set; }

    public Card RulingCard { get; private set; }

    public Player ActivePlayer { get; private set; }

    public Player PlayerWhoKnocked { get; private set; }

    public GameStates State { get; private set; }

    public Round(List<Player> players, Player? activePlayer = null)
    {
        Id = Interlocked.Increment(ref _nextId);
        Players = players;
        if (Id == 1 || activePlayer == null)
        {
            Random random = new Random();
            int randomIndex = random.Next(0, Players.Count);
            ActivePlayer = Players[randomIndex];
        }
        else
        {
            ActivePlayer = activePlayer;
        }
    }

    public StatusMessage Knock(Player player)
    {
        if (player != ActivePlayer)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.NotPlayersTurn,
            };
        }

        if (State != GameStates.WaitingForCardOrKnock)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.CantPerformActionDuringThisGameState,
            };
        }

        PlayerWhoKnocked = player;
        State = GameStates.PlayerKnocked;
        NextPlayer();

        return new StatusMessage
        {
            Success = true
        };
    }

    public StatusMessage Fold(Player player)
    {
        if (player != ActivePlayer)
        {
            return new StatusMessage(false, Messages.NotPlayersTurn);
        }

        if (State != GameStates.PlayerKnocked)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.CantPerformActionDuringThisGameState,
            };
        }

        player.Folds();
        return new StatusMessage();
    }

    public StatusMessage PlayCard(Player player, Card card)
    {
        if (player != ActivePlayer)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.NotPlayersTurn,
            };
        }

        if (State != GameStates.WaitingForCardOrKnock)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.CantPerformActionDuringThisGameState,
            };
        }

        player.PlayCard(card);
        NextPlayer();

        return new StatusMessage
        {
            Success = true,
        };
    }

    private void NextPlayer()
    {
        int currentIndex = Players.IndexOf(ActivePlayer);
        int nextIndex = (currentIndex + 1) % Players.Count;
        ActivePlayer = Players[nextIndex];
    }
}
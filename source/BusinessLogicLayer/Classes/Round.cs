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
            return new StatusMessage(false, Messages.NotPlayersTurn);
        }

        if (State != GameStates.WaitingForCardOrKnock)
        {
            return new StatusMessage(false, Messages.CantPerformActionDuringThisGameState);
        }

        PlayerWhoKnocked = player;
        State = GameStates.PlayerKnocked;
        NextPlayer();

        return new StatusMessage(true);
    }

    public StatusMessage Fold(Player player)
    {
        if (State != GameStates.PlayerKnocked)
            return new StatusMessage(false, Messages.CantPerformActionDuringThisGameState);
        
        if (player != ActivePlayer)
            return new StatusMessage(false, Messages.NotPlayersTurn);

        if (player == PlayerWhoKnocked)
            return new StatusMessage(false, Messages.CantDoThisActionOnYourself);
        
        if (player.Folded)
            return new StatusMessage(false, Messages.AlreadyFolded);

        player.Folds();
        NextPlayer();
        
        return new StatusMessage(true);
    }
    
    public StatusMessage Check(Player player)
    {
        if (State != GameStates.PlayerKnocked)
            return new StatusMessage(false, Messages.CantPerformActionDuringThisGameState);
        
        if (player != ActivePlayer)
            return new StatusMessage(false, Messages.NotPlayersTurn);

        if (player == PlayerWhoKnocked)
            return new StatusMessage(false, Messages.CantDoThisActionOnYourself);
        
        if (player.Folded)
            return new StatusMessage(false, Messages.AlreadyFolded);

        NextPlayer();
        
        return new StatusMessage(true);
    }

    public StatusMessage PlayCard(Player player, Card card)
    {
        if (player != ActivePlayer)
        {
            return new StatusMessage(false, Messages.NotPlayersTurn);
        }

        if (State != GameStates.WaitingForCardOrKnock)
        {
            return new StatusMessage(false, Messages.CantPerformActionDuringThisGameState);
        }

        player.PlayCard(card);
        NextPlayer();

        return new StatusMessage(true);
    }

    private void NextPlayer(int i = 0)
    {
        if (i > Players.Count)
        {
            //TODO: alle spelers zijn gefold
            throw new NotImplementedException();
        }

        int currentIndex = Players.IndexOf(ActivePlayer);
        int nextIndex = (currentIndex + 1) % Players.Count;
        Player newActivePlayer = Players[nextIndex];
        if (newActivePlayer.Folded)
        {
            NextPlayer(i + 1);
        }
    }
}
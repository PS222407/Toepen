using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Round
{
    static int _nextId;

    public int Id { get; }

    public List<Player> Players { get; private set; }

    public Card StartedCard { get; private set; }

    public Player StartedPlayer { get; private set; }

    public Player ActivePlayer { get; private set; }

    public Player PlayerWhoKnocked { get; private set; }

    public Player Winner { get; private set; }

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

        StartedPlayer = ActivePlayer;
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
        
        if (player == PlayerWhoKnocked)
        {
            return new StatusMessage(false, Messages.CantDoThisActionOnYourself);
        }

        PlayerWhoKnocked = player;
        State = GameStates.PlayerKnocked;
        SetNextPlayer();

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
        SetNextPlayer();

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

        SetNextPlayer();

        return new StatusMessage(true);
    }

    public StatusMessage PlayCard(Player player, Card card)
    {
        if (player != ActivePlayer)
        {
            return new StatusMessage(false, Messages.NotPlayersTurn);
        }

        if (State == GameStates.PlayerKnocked && player == PlayerWhoKnocked)
        {
            State = GameStates.WaitingForCardOrKnock;
        }

        if (State != GameStates.WaitingForCardOrKnock)
        {
            return new StatusMessage(false, Messages.CantPerformActionDuringThisGameState);
        }
        
        player.PlayCard(card);
        SetNextPlayer();

        return new StatusMessage(true);
    }

    private void SetNextPlayer()
    {
        int currentIndex = Players.IndexOf(ActivePlayer);
        int nextIndex = (currentIndex + 1) % Players.Count;
        Player nextPlayer = Players[nextIndex];

        Player? winner = CheckRoundForAnyWinner(nextPlayer);
        if (winner != null)
        {
            Winner = winner;
            return;
        }
        
        if (nextPlayer.IsOutOfGame())
        {
            SetNextPlayer();
        }
        else
        {
            ActivePlayer = nextPlayer;
        }
    }

    private Player? CheckRoundForAnyWinner(Player player)
    {
        List<Player> playersStillInGame = Players.Where(p => !p.IsOutOfGame()).ToList();
        if (playersStillInGame.Count == 1)
        {
            return playersStillInGame.First();
        }

        if (StartedPlayer == player)
        {
            Player winner = StartedPlayer;
            foreach (Player p in playersStillInGame)
            {
                Card pCard = p.PlayedCards.Last();
                if (pCard.Suit == StartedCard.Suit && pCard.Value > StartedCard.Value)
                {
                    winner = p;
                }
            }

            return winner;
        }

        return null;
    }
}
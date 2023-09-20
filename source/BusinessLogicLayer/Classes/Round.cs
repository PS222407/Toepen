using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Round
{
    static int _nextId;

    public int Id { get; }

    public List<Player> Players { get; private set; }

    public Card? StartedCard { get; private set; }

    public Player StartedPlayer { get; private set; }

    public Player ActivePlayer { get; private set; }

    public Player PlayerWhoKnocked { get; private set; }

    public WinnerStatus? WinnerStatus { get; private set; }

    public GameState? State { get; private set; }

    public int PenaltyPoints { get; private set; } = 1;

    public Round(List<Player> players)
    {
        Id = Interlocked.Increment(ref _nextId);

        Players = players;

        Random random = new Random();
        int randomIndex = random.Next(0, Players.Count);
        ActivePlayer = Players[randomIndex];

        StartedPlayer = ActivePlayer;

        State = GameState.WaitingForCardOrKnock;
    }

    public Round(List<Player> players, Player previousWinner, int penaltyPoints)
    {
        Id = Interlocked.Increment(ref _nextId);

        PenaltyPoints = penaltyPoints;

        Players = players;

        ActivePlayer = previousWinner;

        StartedPlayer = ActivePlayer;

        State = GameState.WaitingForCardOrKnock;
    }

    public StatusMessage Knock(Player player)
    {
        if (player != ActivePlayer)
        {
            return new StatusMessage(false, Message.NotPlayersTurn);
        }

        if (State != GameState.WaitingForCardOrKnock)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        if (player == PlayerWhoKnocked)
        {
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);
        }

        PlayerWhoKnocked = player;
        State = GameState.PlayerKnocked;
        SetNextPlayer();

        return new StatusMessage(true);
    }

    public StatusMessage Fold(Player player)
    {
        if (State != GameState.PlayerKnocked)
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);

        if (player != ActivePlayer)
            return new StatusMessage(false, Message.NotPlayersTurn);

        if (player == PlayerWhoKnocked)
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);

        if (player.Folded)
            return new StatusMessage(false, Message.AlreadyFolded);

        player.Folds();
        player.AddPenaltyPoints(PenaltyPoints);
        SetNextPlayer();

        return new StatusMessage(true);
    }

    public StatusMessage Check(Player player)
    {
        if (State != GameState.PlayerKnocked)
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);

        if (player != ActivePlayer)
            return new StatusMessage(false, Message.NotPlayersTurn);

        if (player == PlayerWhoKnocked)
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);

        if (player.Folded)
            return new StatusMessage(false, Message.AlreadyFolded);

        SetNextPlayer();

        return new StatusMessage(true);
    }

    public StatusMessage PlayCard(Player player, Card card)
    {
        if (State != GameState.WaitingForCardOrKnock)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        if (player != ActivePlayer)
        {
            return new StatusMessage(false, Message.NotPlayersTurn);
        }

        if (!player.Hand.Any(c => c.Value == card.Value && c.Suit == card.Suit))
        {
            return new StatusMessage(false, Message.CardNotInPlayersHand);
        }

        if (StartedCard != null && player.Hand.Any(c => c.Suit == StartedCard.Suit) && card.Suit != StartedCard.Suit)
        {
            return new StatusMessage(false, Message.PlayerHasMatchingSuitCard);
        }

        player.PlayCard(card);
        StartedCard ??= card;
        SetNextPlayer();

        return new StatusMessage(true);
    }

    private void SetNextPlayer()
    {
        int currentIndex = Players.IndexOf(ActivePlayer);
        int nextIndex = (currentIndex + 1) % Players.Count;
        Player nextPlayer = Players[nextIndex];

        WinnerStatus? winnerStatus = CheckRoundForAnyWinner(nextPlayer);
        if (winnerStatus != null)
        {
            WinnerStatus = winnerStatus;
            return;
        }

        CheckIfKnockRoundIsOver(nextPlayer);

        if (nextPlayer.IsOutOfGame())
        {
            SetNextPlayer();
        }
        else
        {
            ActivePlayer = nextPlayer;
        }
    }

    private void CheckIfKnockRoundIsOver(Player nextPlayer)
    {
        if (State == GameState.PlayerKnocked && nextPlayer == PlayerWhoKnocked)
        {
            State = GameState.WaitingForCardOrKnock;
            PenaltyPoints++;
        }
    }

    private WinnerStatus? CheckRoundForAnyWinner(Player player)
    {
        List<Player> playersStillInGame = Players.Where(p => !p.IsOutOfGame()).ToList();
        if (playersStillInGame.Count == 1)
        {
            return new WinnerStatus(playersStillInGame.First(), true);
        }

        if (StartedPlayer == player)
        {
            Player winner = StartedPlayer;
            foreach (Player p in playersStillInGame)
            {
                Card card = p.PlayedCards.Last();
                if (card.Suit == StartedCard.Suit && card.Value > StartedCard.Value)
                {
                    winner = p;
                }
            }

            foreach (Player pl in playersStillInGame)
            {
                if (pl != winner)
                {
                    pl.AddPenaltyPoints(PenaltyPoints);
                }
            }

            return new WinnerStatus(winner, false);
        }

        return null;
    }
}
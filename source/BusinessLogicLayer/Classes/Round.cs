using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Round
{
    private Card? _startedCard;
    
    private Player _playerWhoKnocked;
    
    private GameState? _state;
    
    public List<Player> Players { get; private set; }

    public Player StartedPlayer { get; private set; }

    public Player ActivePlayer { get; private set; }

    public WinnerStatus? WinnerStatus { get; private set; }

    public int PenaltyPoints { get; private set; } = 1;

    public Round(List<Player> players)
    {
        Players = players;

        Random random = new Random();
        int randomIndex = random.Next(0, Players.Count);
        ActivePlayer = Players[randomIndex];

        StartedPlayer = ActivePlayer;

        _state = GameState.WaitingForCardOrKnock;
    }

    public Round(List<Player> players, Player previousWinner, int penaltyPoints)
    {
        PenaltyPoints = penaltyPoints;

        Players = players;

        ActivePlayer = previousWinner;

        StartedPlayer = ActivePlayer;

        _state = GameState.WaitingForCardOrKnock;
    }

    public StatusMessage Knock(Player player)
    {
        if (player != ActivePlayer)
        {
            return new StatusMessage(false, Message.NotPlayersTurn);
        }

        if (_state != GameState.WaitingForCardOrKnock)
        {
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);
        }

        if (player == _playerWhoKnocked)
        {
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);
        }

        _playerWhoKnocked = player;
        _state = GameState.PlayerKnocked;
        SetNextPlayer();

        return new StatusMessage(true);
    }

    public StatusMessage Fold(Player player)
    {
        if (_state != GameState.PlayerKnocked)
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);

        if (player != ActivePlayer)
            return new StatusMessage(false, Message.NotPlayersTurn);

        if (player == _playerWhoKnocked)
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
        if (_state != GameState.PlayerKnocked)
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);

        if (player != ActivePlayer)
            return new StatusMessage(false, Message.NotPlayersTurn);

        if (player == _playerWhoKnocked)
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);

        if (player.Folded)
            return new StatusMessage(false, Message.AlreadyFolded);

        SetNextPlayer();

        return new StatusMessage(true);
    }

    public StatusMessage PlayCard(Player player, Card card)
    {
        if (_state != GameState.WaitingForCardOrKnock)
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

        if (_startedCard != null && player.Hand.Any(c => c.Suit == _startedCard.Suit) && card.Suit != _startedCard.Suit)
        {
            return new StatusMessage(false, Message.PlayerHasMatchingSuitCard);
        }

        player.PlayCard(card);
        _startedCard ??= card;
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
        if (_state == GameState.PlayerKnocked && nextPlayer == _playerWhoKnocked)
        {
            _state = GameState.WaitingForCardOrKnock;
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
                if (card.Suit == _startedCard.Suit && card.Value > _startedCard.Value)
                {
                    winner = p;
                }
            }

            return new WinnerStatus(winner, false);
        }

        return null;
    }
}
using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Helpers;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Round
{
    private Card? _startedCard;

    private Player _playerWhoKnocked;

    public GameState? State;

    public List<Player> Players { get; private set; }

    public Player StartedPlayer { get; private set; }

    public Player ActivePlayer { get; private set; }

    public WinnerStatus? WinnerStatus { get; private set; }

    public int PenaltyPoints { get; private set; } = 1;

    private List<Card> _table = new();

    public Round(List<Player> players)
    {
        Players = players;

        Random random = new Random();
        int randomIndex = random.Next(0, Players.Count);
        ActivePlayer = Players[randomIndex];

        StartedPlayer = ActivePlayer;

        State = GameState.WaitingForCardOrKnock;
    }

    public Round(List<Player> players, Player previousWinner, int penaltyPoints, bool fromNewSet = false)
    {
        PenaltyPoints = penaltyPoints;
        Players = players;
        ActivePlayer = previousWinner;

        if (fromNewSet || ActivePlayer.IsOutOfGame())
        {
            SetNextPlayer();
        }

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

        if (player == _playerWhoKnocked)
        {
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);
        }

        _playerWhoKnocked = player;
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

        if (player == _playerWhoKnocked)
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);

        if (player.HasFolded)
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

        if (player == _playerWhoKnocked)
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);

        if (player.HasFolded)
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

        if (_startedCard != null && player.Hand.Any(c => c.Suit == _startedCard.Suit) && card.Suit != _startedCard.Suit)
        {
            return new StatusMessage(false, Message.PlayerHasMatchingSuitCard);
        }

        player.PlayCard(card);
        _table.Add(card);
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

        ActivePlayer = nextPlayer;
        if (nextPlayer.IsOutOfGame())
        {
            SetNextPlayer();
        }
    }

    private void CheckIfKnockRoundIsOver(Player nextPlayer)
    {
        if (State == GameState.PlayerKnocked && nextPlayer == _playerWhoKnocked)
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
            return new WinnerStatus { Winner = playersStillInGame.First(), WinnerOfSet = true };
        }

        if (StartedPlayer == player && State != GameState.PlayerKnocked)
        {
            Card winningCard = _table.Where(card => card.Suit == _startedCard.Suit && card.Value >= _startedCard.Value).OrderByDescending(card => card.Value).First();
            Player winner = Players.First(p => p.PlayedCards.Any(pc => pc.Suit == winningCard.Suit && pc.Value == winningCard.Value));

            return new WinnerStatus { Winner = winner, WinnerOfSet = false };
        }

        return null;
    }
}
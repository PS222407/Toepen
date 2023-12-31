﻿using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Helpers;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Round
{
    public Card? StartedCard { get; private set; }

    public Player? PlayerWhoKnocked { get; private set; }

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

        Random random = new();
        int randomIndex = random.Next(0, Players.Count);
        ActivePlayer = Players[randomIndex];

        if (ActivePlayer.IsOutOfGame())
        {
            SetNextPlayer();
        }
        
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

    public void MoveOnToNextSet(Player player)
    {
        if (player.HasCalledMoveOnToNextSet)
        {
            throw new PlayerAlreadyCalledMoveOnToNextSetException();
        }

        player.CallsMoveOnToNextSet();
    }

    /// <exception cref="NotPlayersTurnException"></exception>
    public void Knock(Player player)
    {
        if (player != ActivePlayer)
        {
            throw new NotPlayersTurnException();
        }

        PlayerWhoKnocked = player;
        State = GameState.PlayerKnocked;
        SetNextPlayer();
    }

    public StatusMessage Fold(Player player)
    {
        if (State != GameState.PlayerKnocked)
            return new StatusMessage(false, Message.CantPerformActionDuringThisGameState);

        if (player != ActivePlayer)
            return new StatusMessage(false, Message.NotPlayersTurn);

        if (player == PlayerWhoKnocked)
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

        if (player == PlayerWhoKnocked)
            return new StatusMessage(false, Message.CantDoThisActionOnYourself);

        if (player.HasFolded)
            return new StatusMessage(false, Message.AlreadyFolded);

        SetNextPlayer();

        return new StatusMessage(true);
    }

    /// <exception cref="NotPlayersTurnException"></exception>
    /// <exception cref="CardDoesNotMatchSuitsException"></exception>
    /// <exception cref="CardNotFoundException"></exception>
    public void PlayCard(Player player, Card card)
    {
        if (player != ActivePlayer)
        {
            throw new NotPlayersTurnException();
        }

        if (StartedCard != null && player.Hand.Any(c => c.Suit == StartedCard.Suit) && card.Suit != StartedCard.Suit)
        {
            throw new CardDoesNotMatchSuitsException();
        }

        player.PlayCard(card);
        _table.Add(card);
        StartedCard ??= card;
        SetNextPlayer();
    }

    public void SetNextPlayer()
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
            return new WinnerStatus { Winner = playersStillInGame.First(), WinnerOfSet = true };
        }

        if (StartedPlayer == player && State != GameState.PlayerKnocked)
        {
            Card? winningCard = _table.Where(card => card.Suit == StartedCard.Suit && card.Value >= StartedCard.Value).OrderByDescending(card => card.Value).FirstOrDefault();
            if (winningCard == null)
            {
                return null;
            }
            
            Player winner = Players.First(p => p.PlayedCards.Any(pc => pc.Suit == winningCard.Suit && pc.Value == winningCard.Value));

            return new WinnerStatus { Winner = winner, WinnerOfSet = false };
        }

        return null;
    }
}
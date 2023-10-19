﻿using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Helpers;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Player
{
    static int _nextId;

    public int Id { get; }

    public string Name { get; private set; }

    private List<Card> _hand = new();
    public IReadOnlyList<Card> Hand => _hand;

    private List<Card> _playedCards = new();
    public IReadOnlyList<Card> PlayedCards => _playedCards;

    public int PenaltyPoints { get; private set; }

    public bool HasFolded { get; private set; }

    public bool HasCalledDirtyLaundry { get; private set; }

    public bool HasCalledWhiteLaundry { get; private set; }

    public bool LaundryHasBeenTurned { get; private set; }

    public bool PlayWithOpenCards { get; private set; }

    public Player(string name)
    {
        Id = Interlocked.Increment(ref _nextId);
        Name = name;
    }

    public void ResetVariablesForNewSet()
    {
        _hand = new List<Card>();
        _playedCards = new List<Card>();
        HasFolded = false;
        HasCalledDirtyLaundry = false;
        HasCalledWhiteLaundry = false;
        LaundryHasBeenTurned = false;
        PlayWithOpenCards = false;
    }

    public void AddPenaltyPoints(int points)
    {
        PenaltyPoints += points;
    }

    public void DealCard(Card card)
    {
        _hand.Add(card);
    }

    public void RemoveCardFromHand(Card card)
    {
        Card? cardFromHand = _hand.FirstOrDefault(c => c.Suit == card.Suit && c.Value == card.Value);
        if (cardFromHand == null)
        {
            throw new CardNotFoundException();
        }

        _hand.Remove(cardFromHand);
    }

    public void CalledDirtyLaundry()
    {
        HasCalledDirtyLaundry = true;
    }

    public void CalledWhiteLaundry()
    {
        HasCalledWhiteLaundry = true;
    }

    public bool TurnsAndChecksDirtyLaundry()
    {
        LaundryHasBeenTurned = true;
        return HasDirtyLaundry();
    }

    public bool HasDirtyLaundry()
    {
        return Hand.Count == Settings.MaxCardsPerHand && Hand.All(card => card.Value < Value.Seven);
    }

    public bool TurnsAndChecksWhiteLaundry()
    {
        LaundryHasBeenTurned = true;
        return HasWhiteLaundry();
    }

    public bool HasWhiteLaundry()
    {
        int sevenCount = 0;
        foreach (Card card in Hand)
        {
            if (card.Value > Value.Seven)
            {
                return false;
            }

            if (card.Value == Value.Seven)
            {
                sevenCount++;
            }
        }

        return sevenCount == 1;
    }

    public void ResetLaundryVariables()
    {
        HasCalledDirtyLaundry = false;
        HasCalledWhiteLaundry = false;
        LaundryHasBeenTurned = false;
    }

    public void MustPlayWithOpenCards()
    {
        PlayWithOpenCards = true;
    }

    public void PlayCard(Card card)
    {
        Card cardFromPlayersHand = Hand.First(c => c.Value == card.Value && c.Suit == card.Suit);
        _hand.Remove(cardFromPlayersHand);
        _playedCards.Add(cardFromPlayersHand);
    }

    public void Folds()
    {
        HasFolded = true;
    }

    public bool IsDead()
    {
        return PenaltyPoints >= Settings.MaxPenaltyPoints;
    }

    public bool IsOutOfGame()
    {
        return IsDead() || HasFolded;
    }

    // TODO: implement in gameflow
    public bool HasPoverty()
    {
        return PenaltyPoints == Settings.MaxPenaltyPoints - 1;
    }
}
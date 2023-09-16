﻿using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Player
{
    static int _nextId;

    public int Id { get; }

    public string Name { get; private set; }

    public List<Card> Hand { get; set; } = new();
    
    public List<Card> PlayedCards { get; private set; } = new();

    public int PenaltyPoints { get; private set; }

    public bool Folded { get; private set; } = false;

    public bool HasCalledDirtyLaundry { get; private set; }
    
    public bool HasCalledWhiteLaundry { get; private set; }
    
    public bool LaundryHasBeenTurned { get; private set; }
    
    public bool PlayWithOpenCards { get; private set; }

    public Player(string name)
    {
        Id = Interlocked.Increment(ref _nextId);
        Name = name;
    }

    public void AddPenaltyPoints(int points)
    {
        PenaltyPoints += points;
    }

    public void DealCard(Card card)
    {
        Hand.Add(card);
    }    
    
    public void RemoveCardFromHand(Card card)
    {
        Hand.Remove(card);
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
        Hand.Remove(cardFromPlayersHand);
        PlayedCards.Add(cardFromPlayersHand);
    }

    public void Folds()
    {
        Folded = true;
    }

    public bool IsDead()
    {
        return PenaltyPoints >= Settings.MaxPenaltyPoints;
    }

    public bool IsOutOfGame()
    {
        return IsDead() || Folded;
    }
}
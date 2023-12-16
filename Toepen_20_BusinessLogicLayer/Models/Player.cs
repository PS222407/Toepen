using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Helpers;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Player
{
    private static int _nextId;

    public int Id { get; }

    public string ConnectionId { get; private set; }

    public string Name { get; private set; }

    private List<Card> _hand = new();
    
    public bool IsHost { get; private set; }

    public IReadOnlyList<Card> Hand => _hand;

    private List<Card> _playedCards = new();

    public IReadOnlyList<Card> PlayedCards => _playedCards;

    public int PenaltyPoints { get; private set; }

    public bool HasFolded { get; private set; }

    public bool HasNoLaundry { get; private set; }

    public bool HasCalledDirtyLaundry { get; private set; }

    public bool HasCalledWhiteLaundry { get; private set; }

    public bool LaundryHasBeenTurned { get; private set; }

    public bool PlayWithOpenCards { get; private set; }

    public bool HasCalledMoveOnToNextSet { get; private set; }

    public Player(string name)
    {
        Id = Interlocked.Increment(ref _nextId);
        Name = name;
    }

    public Player(string connectionId, string name)
    {
        Id = Interlocked.Increment(ref _nextId);
        ConnectionId = connectionId;
        Name = name;
    }
    
    public void SetAsHost()
    {
        IsHost = true;
    }

    public void ResetVariablesForNewSet()
    {
        _hand = new List<Card>();
        _playedCards = new List<Card>();
        HasFolded = false;
        HasNoLaundry = false;
        HasCalledDirtyLaundry = false;
        HasCalledWhiteLaundry = false;
        LaundryHasBeenTurned = false;
        PlayWithOpenCards = false;
        HasCalledMoveOnToNextSet = false;
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

    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void CallsDirtyLaundry()
    {
        if (HasCalledWhiteLaundry || HasCalledDirtyLaundry || HasNoLaundry)
        {
            throw new AlreadyCalledLaundryException();
        }

        HasCalledDirtyLaundry = true;
    }

    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void CallsWhiteLaundry()
    {
        if (HasCalledWhiteLaundry || HasCalledDirtyLaundry || HasNoLaundry)
        {
            throw new AlreadyCalledLaundryException();
        }

        HasCalledWhiteLaundry = true;
    }

    /// <exception cref="AlreadyCalledLaundryException"></exception>
    public void CallsNoLaundry()
    {
        if (HasCalledWhiteLaundry || HasCalledDirtyLaundry || HasNoLaundry)
        {
            throw new AlreadyCalledLaundryException();
        }

        HasNoLaundry = true;
    }

    public bool HasDirtyLaundry()
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

    public bool HasWhiteLaundry()
    {
        return Hand.Count == Settings.MaxCardsPerHand && Hand.All(card => card.Value < Value.Seven);
    }

    public bool TurnsAndChecksDirtyLaundry()
    {
        LaundryHasBeenTurned = true;
        return HasDirtyLaundry();
    }

    public bool TurnsAndChecksWhiteLaundry()
    {
        LaundryHasBeenTurned = true;
        return HasWhiteLaundry();
    }

    public void ResetLaundryVariables()
    {
        HasNoLaundry = false;
        HasCalledDirtyLaundry = false;
        HasCalledWhiteLaundry = false;
        LaundryHasBeenTurned = false;
    }

    public void MustPlayWithOpenCards()
    {
        PlayWithOpenCards = true;
    }

    /// <exception cref="CardNotFoundException"></exception>
    public void PlayCard(Card card)
    {
        Card? cardFromHand = _hand.FirstOrDefault(c => c.Suit == card.Suit && c.Value == card.Value);
        if (cardFromHand == null)
        {
            throw new CardNotFoundException();
        }

        _hand.Remove(cardFromHand);
        _playedCards.Add(cardFromHand);
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

    public void CallsMoveOnToNextSet()
    {
        HasCalledMoveOnToNextSet = true;
    }
}
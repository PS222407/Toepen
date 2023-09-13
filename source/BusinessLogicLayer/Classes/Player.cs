using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Player
{
    static int _nextId;

    public int Id { get; }

    public string Name { get; private set; }

    public List<Card> Hand { get; private set; } = new();

    public int PenaltyPoints { get; private set; }

    public Player(string name)
    {
        Id = Interlocked.Increment(ref _nextId);
        Name = name;
    }

    public void DealCard(Card card)
    {
        Hand.Add(card);
    }

    public bool HasDirtyLaundry()
    {
        return Hand.All(card => card.Value < Values.Seven);
    }

    public bool HasWhiteLaundry()
    {
        int sevenCount = 0;
        foreach (Card card in Hand)
        {
            if (card.Value > Values.Seven)
            {
                return false;
            }
            if (card.Value == Values.Seven)
            {
                sevenCount++;
            }
        }

        return sevenCount <= 1;
    }
}
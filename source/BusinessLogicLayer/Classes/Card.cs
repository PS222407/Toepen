using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Card
{
    public Suits Suit { get; private set; }

    public Values Value { get; private set; }

    public Card(Suits suit, Values value)
    {
        Suit = suit;
        Value = value;
    }

    public override string ToString()
    {
        return $"{(int)Value} - {Value} of {Suit}";
    }
}
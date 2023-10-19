using Toepen_20_BusinessLogicLayer.Enums;

namespace Toepen_20_BusinessLogicLayer.Models;

public class Card
{
    public Suit Suit { get; private set; }

    public Value Value { get; private set; }

    public Card(Suit suit, Value value)
    {
        Suit = suit;
        Value = value;
    }

    public override string ToString()
    {
        return $"{(int)Value} - {Value} of {Suit}";
    }
}
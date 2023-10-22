using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Models;
using UnitTests.Utilities;

namespace UnitTests;

[TestFixture]
public class PlayerTests
{
    [Test]
    public void PlayerHasWhiteLaundry_ReturnsTrue()
    {
        // Arrange
        Player playerWithWhiteLaundry = new("Test player");
        List<Card> cards = new()
        {
            new Card(Suit.Spades, Value.Ace),
            new Card(Suit.Diamonds, Value.King),
            new Card(Suit.Clubs, Value.Seven),
            new Card(Suit.Diamonds, Value.Ace)
        };
        Entity.SetHandOf(playerWithWhiteLaundry, cards);

        // Act
        bool result = playerWithWhiteLaundry.HasWhiteLaundry();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void PlayerHasDirtyLaundry_ReturnsTrue()
    {
        // Arrange
        Player playerWithDirtyLaundry = new("Test player");
        List<Card> cards = new()
        {
            new Card(Suit.Hearts, Value.Jack),
            new Card(Suit.Hearts, Value.King),
            new Card(Suit.Diamonds, Value.Jack),
            new Card(Suit.Hearts, Value.Ace)
        };
        Entity.SetHandOf(playerWithDirtyLaundry, cards);

        // Act
        bool result = playerWithDirtyLaundry.HasDirtyLaundry();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void PlayerHasNotWhiteLaundry_ReturnsFalse()
    {
        // Arrange
        Player playerWithoutWhiteLaundry = new("Test player");
        List<Card> cards = new()
        {
            new Card(Suit.Clubs, Value.Nine),
            new Card(Suit.Diamonds, Value.Nine),
            new Card(Suit.Diamonds, Value.Queen),
            new Card(Suit.Hearts, Value.Queen)
        };
        Entity.SetHandOf(playerWithoutWhiteLaundry, cards);

        // Act
        bool result = playerWithoutWhiteLaundry.HasWhiteLaundry();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void PlayerHasNotDirtyLaundry_ReturnsFalse()
    {
        // Arrange
        Player playerWithoutDirtyLaundry = new("Test player");
        List<Card> cards = new()
        {
            new Card(Suit.Clubs, Value.Nine),
            new Card(Suit.Diamonds, Value.Nine),
            new Card(Suit.Diamonds, Value.Queen),
            new Card(Suit.Hearts, Value.Queen)
        };
        Entity.SetHandOf(playerWithoutDirtyLaundry, cards);

        // Act
        bool result = playerWithoutDirtyLaundry.HasDirtyLaundry();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void PlayerResetVariablesForNewSet_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");

        Entity.SetHandOf(player, new List<Card>
        {
            new(Suit.Clubs, Value.Ace),
            new(Suit.Diamonds, Value.Ace),
            new(Suit.Spades, Value.Ace),
            new(Suit.Hearts, Value.Ace)
        });
        Entity.SetPlayedCardsOf(player, new List<Card>
        {
            new(Suit.Clubs, Value.Ace),
            new(Suit.Diamonds, Value.Ace),
            new(Suit.Spades, Value.Ace),
            new(Suit.Hearts, Value.Ace)
        });
        Entity.SetHasFoldedOf(player, true);
        Entity.SetHasCalledDirtyLaundryOf(player, true);
        Entity.SetHasCalledWhiteLaundryOf(player, true);
        Entity.SetLaundryHasBeenTurnedOf(player, true);
        Entity.SetPlayWithOpenCardsOf(player, true);

        Assert.That(
            player.Hand.Count == 4 &&
            player.PlayedCards.Count == 4 &&
            player.HasFolded &&
            player.HasCalledDirtyLaundry &&
            player.HasCalledWhiteLaundry &&
            player.LaundryHasBeenTurned &&
            player.PlayWithOpenCards
        );

        // Act
        player.ResetVariablesForNewSet();

        // Assert
        Assert.That(
            player.Hand.Count == 0 &&
            player.PlayedCards.Count == 0 &&
            player.HasFolded == false &&
            player.HasCalledDirtyLaundry == false &&
            player.HasCalledWhiteLaundry == false &&
            player.LaundryHasBeenTurned == false &&
            player.PlayWithOpenCards == false
        );
    }

    [Test]
    public void PlayerResetLaundryVariables_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");

        Entity.SetHasCalledDirtyLaundryOf(player, true);
        Entity.SetHasCalledWhiteLaundryOf(player, true);
        Entity.SetLaundryHasBeenTurnedOf(player, true);

        Assert.That(player is { HasCalledDirtyLaundry: true, HasCalledWhiteLaundry: true, LaundryHasBeenTurned: true });

        // Act
        player.ResetLaundryVariables();

        // Assert
        Assert.That(player is { HasCalledDirtyLaundry: false, HasCalledWhiteLaundry: false, LaundryHasBeenTurned: false });
    }

    [Test]
    public void PlayerAddPenaltyPoints_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");

        // Act
        player.AddPenaltyPoints(2);

        // Assert
        Assert.That(player.PenaltyPoints, Is.EqualTo(2));
    }

    [Test]
    public void PlayerGetDealtCards_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");

        // Act
        player.DealCard(new Card(Suit.Diamonds, Value.Eight));

        // Assert
        Assert.That(
            player.Hand.Count == 1 &&
            player.Hand[0].Suit == Suit.Diamonds &&
            player.Hand[0].Value == Value.Eight
        );
    }

    [Test]
    public void PlayerRemovedOneCardFromHand_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");

        // Act
        player.DealCard(new Card(Suit.Diamonds, Value.Eight));
        player.RemoveCardFromHand(new Card(Suit.Diamonds, Value.Eight));

        // Assert
        Assert.That(player.Hand.Count, Is.EqualTo(0));
    }

    [Test]
    public void PlayerCalledDirtyLaundry_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");

        // Act
        player.CallsDirtyLaundry();

        // Assert
        Assert.That(player.HasCalledDirtyLaundry, Is.True);
    }

    [Test]
    public void PlayerCalledWhiteLaundry_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");

        // Act
        player.CallsWhiteLaundry();

        // Assert
        Assert.That(player.HasCalledWhiteLaundry, Is.True);
    }

    [Test]
    public void PlayerDirtyLaundryGetsTurned_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");
        List<Card> cards = new()
        {
            new Card(Suit.Clubs, Value.Jack),
            new Card(Suit.Spades, Value.Jack),
            new Card(Suit.Diamonds, Value.Jack),
            new Card(Suit.Hearts, Value.Jack)
        };
        Entity.SetHandOf(player, cards);

        // Act
        bool result = player.TurnsAndChecksDirtyLaundry();

        // Assert
        Assert.That(player.LaundryHasBeenTurned && result, Is.True);
    }

    [Test]
    public void PlayerWhiteLaundryGetsTurned_ReturnsTrue()
    {
        // Arrange
        Player player = new("Test Player");
        List<Card> cards = new()
        {
            new Card(Suit.Clubs, Value.Jack),
            new Card(Suit.Spades, Value.Jack),
            new Card(Suit.Diamonds, Value.Seven),
            new Card(Suit.Hearts, Value.Jack)
        };
        Entity.SetHandOf(player, cards);

        // Act
        bool result = player.TurnsAndChecksWhiteLaundry();

        // Assert
        Assert.That(player.LaundryHasBeenTurned && result, Is.True);
    }

    // TODO: MustPlayWithOpenCards():void
    // TODO: PlayCard(Card card):void
    // TODO: Folds():void
    // TODO: IsDead():bool
    // TODO: IsOutOfGame():bool
    // TODO: HasPoverty():bool
}
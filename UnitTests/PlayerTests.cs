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
        Player playerWithWhiteLaundry = new Player("Test player");
        List<Card> cards = new List<Card>
        {
            new(Suit.Spades, Value.Ace),
            new(Suit.Diamonds, Value.King),
            new(Suit.Clubs, Value.Seven),
            new(Suit.Diamonds, Value.Ace),
        };
        Entity.SetHandOf(playerWithWhiteLaundry, cards);

        // Act
        bool result = playerWithWhiteLaundry.HasWhiteLaundry();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void PlayerHasDirtyLaundry_ReturnsTrue()
    {
        // Arrange
        Player playerWithDirtyLaundry = new Player("Test player");
        List<Card> cards = new List<Card>
        {
            new(Suit.Hearts, Value.Jack),
            new(Suit.Hearts, Value.King),
            new(Suit.Diamonds, Value.Jack),
            new(Suit.Hearts, Value.Ace),
        };
        Entity.SetHandOf(playerWithDirtyLaundry, cards);

        // Act
        bool result = playerWithDirtyLaundry.HasDirtyLaundry();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void PlayerHasNotWhiteLaundry_ReturnsFalse()
    {
        // Arrange
        Player playerWithoutWhiteLaundry = new Player("Test player");
        List<Card> cards = new List<Card>
        {
            new(Suit.Clubs, Value.Nine),
            new(Suit.Diamonds, Value.Nine),
            new(Suit.Diamonds, Value.Queen),
            new(Suit.Hearts, Value.Queen),
        };
        Entity.SetHandOf(playerWithoutWhiteLaundry, cards);

        // Act
        bool result = playerWithoutWhiteLaundry.HasWhiteLaundry();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void PlayerHasNotDirtyLaundry_ReturnsFalse()
    {
        // Arrange
        Player playerWithoutDirtyLaundry = new Player("Test player");
        List<Card> cards = new List<Card>
        {
            new(Suit.Clubs, Value.Nine),
            new(Suit.Diamonds, Value.Nine),
            new(Suit.Diamonds, Value.Queen),
            new(Suit.Hearts, Value.Queen),
        };
        Entity.SetHandOf(playerWithoutDirtyLaundry, cards);

        // Act
        bool result = playerWithoutDirtyLaundry.HasDirtyLaundry();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void PlayerResetVariablesForNewSet_ReturnsTrue()
    {
        // Arrange
        Player player = new Player("Test Player");

        Entity.SetHandOf(player, new List<Card>
        {
            new(Suit.Clubs, Value.Ace),
            new(Suit.Diamonds, Value.Ace),
            new(Suit.Spades, Value.Ace),
            new(Suit.Hearts, Value.Ace),
        });
        Entity.SetPlayedCardsOf(player, new List<Card>
        {
            new(Suit.Clubs, Value.Ace),
            new(Suit.Diamonds, Value.Ace),
            new(Suit.Spades, Value.Ace),
            new(Suit.Hearts, Value.Ace),
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
        Player player = new Player("Test Player");

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
        Player player = new Player("Test Player");

        // Act
        player.AddPenaltyPoints(2);

        // Assert
        Assert.That(player.PenaltyPoints, Is.EqualTo(2));
    }

    [Test]
    public void PlayerGetDealtCards_ReturnsTrue()
    {
        // Arrange
        Player player = new Player("Test Player");

        // Act
        player.DealCard(new Card(Suit.Diamonds, Value.Eight));

        // Assert
        Assert.That(
            player.Hand.Count == 1 &&
            player.Hand.First().Suit == Suit.Diamonds &&
            player.Hand.First().Value == Value.Eight
        );
    }

    [Test]
    public void PlayerRemovedOneCardFromHand_ReturnsTrue()
    {
        // Arrange
        Player player = new Player("Test Player");

        // Act
        player.DealCard(new Card(Suit.Diamonds, Value.Eight));
        player.RemoveCardFromHand(new Card(Suit.Diamonds, Value.Eight));

        // Assert
        Assert.That(player.Hand.Count == 0);
    }
    
    [Test]
    public void PlayerCalledDirtyLaundry_ReturnsTrue()
    {
        // Arrange
        Player player = new Player("Test Player");

        // Act
        player.CalledDirtyLaundry();

        // Assert
        Assert.IsTrue(player.HasCalledDirtyLaundry);
    }
    
    [Test]
    public void PlayerCalledWhiteLaundry_ReturnsTrue()
    {
        // Arrange
        Player player = new Player("Test Player");

        // Act
        player.CalledWhiteLaundry();

        // Assert
        Assert.IsTrue(player.HasCalledWhiteLaundry);
    }
    
    [Test]
    public void PlayerDirtyLaundryGetsTurned_ReturnsTrue()
    {
        // Arrange
        Player player = new Player("Test Player");
        List<Card> cards = new List<Card>
        {
            new(Suit.Clubs, Value.Jack),
            new(Suit.Spades, Value.Jack),
            new(Suit.Diamonds, Value.Jack),
            new(Suit.Hearts, Value.Jack),
        };
        Entity.SetHandOf(player, cards);

        // Act
        bool result = player.TurnsAndChecksDirtyLaundry();

        // Assert
        Assert.IsTrue(player.LaundryHasBeenTurned && result);
    }
    
    [Test]
    public void PlayerWhiteLaundryGetsTurned_ReturnsTrue()
    {
        // Arrange
        Player player = new Player("Test Player");
        List<Card> cards = new List<Card>
        {
            new(Suit.Clubs, Value.Jack),
            new(Suit.Spades, Value.Jack),
            new(Suit.Diamonds, Value.Seven),
            new(Suit.Hearts, Value.Jack),
        };
        Entity.SetHandOf(player, cards);

        // Act
        bool result = player.TurnsAndChecksWhiteLaundry();

        // Assert
        Assert.IsTrue(player.LaundryHasBeenTurned && result);
    }

    // MustPlayWithOpenCards():void
    // PlayCard(Card card):void
    // Folds():void
    // IsDead():bool
    // IsOutOfGame():bool
    // HasPoverty():bool
}
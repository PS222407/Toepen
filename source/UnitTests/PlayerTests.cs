using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Enums;
using UnitTests.Utilities;

namespace UnitTests;

[TestFixture]
public class PlayerTests
{
    private Game _game;
    
    [SetUp]
    public void Setup()
    {
        _game = new();

        List<Player> players = new()
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
        };
        
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            Entity.SetIdOf(player, i + 1);
            _game.AddPlayer(player);
        }

        _game.Start();

        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];

            if (i == 0)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Spades, Value.Ace),
                    new(Suit.Diamonds, Value.King),
                    new(Suit.Clubs, Value.Seven),
                    new(Suit.Diamonds, Value.Ace),
                };
                Entity.SetHandOf(player, cards);
            }
            if (i == 1)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Hearts, Value.Jack),
                    new(Suit.Hearts, Value.King),
                    new(Suit.Diamonds, Value.Jack),
                    new(Suit.Hearts, Value.Ace),
                };
                Entity.SetHandOf(player, cards);
            }
            if (i == 2)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Clubs, Value.Nine),
                    new(Suit.Diamonds, Value.Nine),
                    new(Suit.Diamonds, Value.Queen),
                    new(Suit.Hearts, Value.Queen),
                };
                Entity.SetHandOf(player, cards);
            }
        }
    }
    
    [Test]
    public void PlayerHasWhiteLaundry_ReturnsTrue()
    {
        // Arrange
        Player playerWithWhiteLaundry = _game.Players[0];
        
        // Act
        bool result = playerWithWhiteLaundry.HasWhiteLaundry();
        
        // Assert
        Assert.IsTrue(result);
    }
    
    [Test]
    public void PlayerHasDirtyLaundry_ReturnsTrue()
    {
        // Arrange
        Player playerWithWhiteLaundry = _game.Players[1];
        
        // Act
        bool result = playerWithWhiteLaundry.HasDirtyLaundry();
        
        // Assert
        Assert.IsTrue(result);
    }
    
    [Test]
    public void PlayerHasNotWhiteLaundry_ReturnsFalse()
    {
        // Arrange
        Player playerWithWhiteLaundry = _game.Players[2];
        
        // Act
        bool result = playerWithWhiteLaundry.HasWhiteLaundry();
        
        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void PlayerHasNotDirtyLaundry_ReturnsFalse()
    {
        // Arrange
        Player playerWithWhiteLaundry = _game.Players[2];
        
        // Act
        bool result = playerWithWhiteLaundry.HasDirtyLaundry();
        
        // Assert
        Assert.IsFalse(result);
    }
}
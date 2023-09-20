using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Enums;

namespace UnitTests;

[TestFixture]
public class GameTests
{
    private Game _game;
    
    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }

    [Test]
    public void TooManyPlayersInGame_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
            new("Niels"),
            new("Bas"),
            new("Gijs"),
            new("Putin"),
        };

        // Act
        bool result = true;
        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                result = false;
            }
        }

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void MaxPlayersInGame_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
            new("Niels"),
            new("Bas"),
            new("Gijs"),
        };

        // Act
        bool result = true;
        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                result = false;
            }
        }

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void MinPlayersInGame_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
        };

        // Act
        bool result = true;
        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                result = false;
            }
        }

        if (_game.Start().Message == Message.MinimumPlayersNotReached)
        {
            result = false;
        }

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void LessThanMinPlayersInGame_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new List<Player>
        {
            new("Sam"),
        };

        bool result = true;
        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                result = false;
            }
        }

        // Act
        if (_game.Start().Message == Message.MinimumPlayersNotReached)
        {
            result = true;
        }

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void GameAlreadyStarted_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
        };

        foreach (Player player in players)
        {
            _game.AddPlayer(player);
        }

        // Act
        bool result = true;
        _game.Start();

        if (_game.Start().Message != Message.GameAlreadyStarted)
        {
            result = false;
        }

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void CardDealtToPlayers_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
        };

        foreach (Player player in players)
        {
            _game.AddPlayer(player);
        }

        // Act
        _game.Start();
        bool result = true;
        foreach (Player player in _game.Players)
        {
            if (player.Hand.Count != 4)
            {
                result = false;
            }
        }

        // Assert
        Assert.IsTrue(result);
    }
}
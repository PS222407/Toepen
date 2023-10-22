using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace UnitTests;

[TestFixture]
public class GameTests
{
    private Game _game;

    [SetUp]
    public void Setup()
    {
        _game = new Game("123");
    }

    [Test]
    public void TooManyPlayersInGame_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new()
        {
            new Player("Sam"),
            new Player("Jens"),
            new Player("Mylo"),
            new Player("Niels"),
            new Player("Bas"),
            new Player("Gijs"),
            new Player("Putin"),
        };

        // Act
        // Assert
        Assert.Throws<TooManyPlayersException>(() =>
        {
            foreach (Player player in players)
            {
                _game.AddPlayer(player);
            }
        });
    }

    [Test]
    public void MaxPlayersInGame_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new()
        {
            new Player("Sam"),
            new Player("Jens"),
            new Player("Mylo"),
            new Player("Niels"),
            new Player("Bas"),
            new Player("Gijs"),
        };

        // Act
        foreach (Player player in players)
        {
            _game.AddPlayer(player);
        }

        // Assert
        Assert.That(_game.Players, Has.Count.EqualTo(6));
    }

    [Test]
    public void MinPlayersInGame_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new()
        {
            new Player("Sam"),
            new Player("Jens"),
        };

        // Act
        foreach (Player player in players)
        {
            _game.AddPlayer(player);
        }

        // Assert
        Assert.That(_game.Players, Has.Count.EqualTo(2));
    }

    [Test]
    public void LessThanMinPlayersInGame_ReturnsTrue()
    {
        // Arrange
        _game.AddPlayer(new Player("Sam"));

        // Act
        // Assert
        Assert.Throws<NotEnoughPlayersException>(() => _game.Start());
    }

    [Test]
    public void GameAlreadyStarted_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new()
        {
            new Player("Sam"),
            new Player("Jens"),
            new Player("Mylo"),
        };

        foreach (Player player in players)
        {
            _game.AddPlayer(player);
        }

        // Act
        _game.Start();

        // Assert
        Assert.Throws<AlreadyStartedException>(() => _game.Start());
    }

    [Test]
    public void CardDealtToPlayers_ReturnsTrue()
    {
        // Arrange
        List<Player> players = new()
        {
            new Player("Sam"),
            new Player("Jens"),
            new Player("Mylo"),
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
        Assert.That(result, Is.True);
    }
}
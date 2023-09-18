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

        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                Assert.Pass();
            }
        }

        Assert.Fail();
    }
    
    [Test]
    public void MaxPlayersInGame_ReturnsTrue()
    {
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
            new("Niels"),
            new("Bas"),
            new("Gijs"),
        };

        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                Assert.Fail();
            }
        }

        Assert.Pass();
    }
    
    [Test]
    public void MinPlayersInGame_ReturnsTrue()
    {
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
        };

        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                Assert.Fail();
            }
        }

        if (_game.Start().Message == Message.MinimumPlayersNotReached)
        {
            Assert.Fail();
        }

        Assert.Pass();
    }
    
    [Test]
    public void LessThanMinPlayersInGame_ReturnsTrue()
    {
        List<Player> players = new List<Player>
        {
            new("Sam"),
        };

        foreach (Player player in players)
        {
            if (!_game.AddPlayer(player))
            {
                Assert.Fail();
            }
        }

        if (_game.Start().Message == Message.MinimumPlayersNotReached)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
    
    [Test]
    public void GameAlreadyStarted_ReturnsTrue()
    {
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

        _game.Start();
            
        if (_game.Start().Message == Message.GameAlreadyStarted)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
    
    [Test]
    public void CardDealtToPlayers_ReturnsTrue()
    {
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

        if (!_game.Start().Success)
        {
            Assert.Fail();
        }

        foreach (Player player in _game.Players)
        {
            if (player.Hand.Count != 4)
            {
                Assert.Fail();
            }
        }
        
        Assert.Pass();
    }
}
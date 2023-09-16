using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Enums;

namespace UnitTests;

[TestFixture]
public class GameTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void TooManyPlayersInGame_ReturnsTrue()
    {
        Game game = new Game();
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
            if (!game.AddPlayer(player))
            {
                Assert.Pass();
            }
        }

        Assert.Fail();
    }
    
    [Test]
    public void MaxPlayersInGame_ReturnsTrue()
    {
        Game game = new Game();
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
            if (!game.AddPlayer(player))
            {
                Assert.Fail();
            }
        }

        Assert.Pass();
    }
    
    [Test]
    public void MinPlayersInGame_ReturnsTrue()
    {
        Game game = new Game();
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
        };

        foreach (Player player in players)
        {
            if (!game.AddPlayer(player))
            {
                Assert.Fail();
            }
        }

        if (game.Start().Message == Message.MinimumPlayersNotReached)
        {
            Assert.Fail();
        }

        Assert.Pass();
    }
    
    [Test]
    public void LessThanMinPlayersInGame_ReturnsTrue()
    {
        Game game = new Game();
        List<Player> players = new List<Player>
        {
            new("Sam"),
        };

        foreach (Player player in players)
        {
            if (!game.AddPlayer(player))
            {
                Assert.Fail();
            }
        }

        if (game.Start().Message == Message.MinimumPlayersNotReached)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
    
    [Test]
    public void GameAlreadyStarted_ReturnsTrue()
    {
        Game game = new Game();
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
        };

        foreach (Player player in players)
        {
            game.AddPlayer(player);
        }

        game.Start();
            
        if (game.Start().Message == Message.GameAlreadyStarted)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
    
    [Test]
    public void CardDealtToPlayers_ReturnsTrue()
    {
        Game game = new Game();
        List<Player> players = new List<Player>
        {
            new("Sam"),
            new("Jens"),
            new("Mylo"),
        };

        foreach (Player player in players)
        {
            game.AddPlayer(player);
        }

        if (!game.Start().Success)
        {
            Assert.Fail();
        }

        foreach (Player player in game.Players)
        {
            if (player.Hand.Count != 4)
            {
                Assert.Fail();
            }
        }
        
        Assert.Pass();
    }
}
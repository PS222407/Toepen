using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Enums;

namespace UnitTests;

[TestFixture]
public class LaundryTests
{
    public Game Game { get; private set; }
    
    [SetUp]
    public void Setup()
    {
        Game = new();

        List<Player> players = new()
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

        //TODO: private set
        for (int i = 0; i < game.Players.Count; i++)
        {
            Player player = game.Players[i];

            if (i == 0)
            {
                player.Hand = new List<Card>
                {
                    new(Suit.Spades, Value.Ace),
                    new(Suit.Diamonds, Value.King),
                    new(Suit.Clubs, Value.Seven),
                    new(Suit.Diamonds, Value.Ace),
                };
            }
            if (i == 1)
            {
                player.Hand = new List<Card>
                {
                    new(Suit.Hearts, Value.Jack),
                    new(Suit.Hearts, Value.King),
                    new(Suit.Diamonds, Value.Jack),
                    new(Suit.Hearts, Value.Ace),
                };
            }
            if (i == 2)
            {
                player.Hand = new List<Card>
                {
                    new(Suit.Clubs, Value.Nine),
                    new(Suit.Diamonds, Value.Nine),
                    new(Suit.Diamonds, Value.Queen),
                    new(Suit.Hearts, Value.Queen),
                };
            }
        }
    }

    [Test]
    public void PlayerHasWhiteLaundry_ReturnsTrue()
    {
        Player? playerWithWhiteLaundry = null;
        foreach (var player in game.Players)
        {
            if (player.Id == 1)
            {
                playerWithWhiteLaundry = player;
            }
        }
        if (playerWithWhiteLaundry == null)
        {
            Assert.Fail();
            return;
        }
        if (playerWithWhiteLaundry.HasDirtyLaundry())
        {
            Assert.Fail();
            return;
        }
        if (playerWithWhiteLaundry.HasWhiteLaundry())
        {
            Assert.Pass();
            return;
        }
    }
}
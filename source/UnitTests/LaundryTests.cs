using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Enums;
using UnitTests.Utilities;

namespace UnitTests;

[TestFixture]
public class LaundryTests
{
    public Game Game { get;  set; }
    
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
            Game.AddPlayer(player);
        }

        Game.Start();

        //TODO: private set
        for (int i = 0; i < Game.Players.Count; i++)
        {
            Player player = Game.Players[i];

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
            // if (i == 1)
            // {
            //     player.Hand = new List<Card>
            //     {
            //         new(Suit.Hearts, Value.Jack),
            //         new(Suit.Hearts, Value.King),
            //         new(Suit.Diamonds, Value.Jack),
            //         new(Suit.Hearts, Value.Ace),
            //     };
            // }
            // if (i == 2)
            // {
            //     player.Hand = new List<Card>
            //     {
            //         new(Suit.Clubs, Value.Nine),
            //         new(Suit.Diamonds, Value.Nine),
            //         new(Suit.Diamonds, Value.Queen),
            //         new(Suit.Hearts, Value.Queen),
            //     };
            // }
        }
    }

    

    [Test]
    public void PlayerHasWhiteLaundry_ReturnsTrue()
    {
        Player? playerWithWhiteLaundry = new Player("Timo");
        Entity.SetIdOf(playerWithWhiteLaundry, 1);
        foreach (var player in Game.Players)
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
        }
    }
}
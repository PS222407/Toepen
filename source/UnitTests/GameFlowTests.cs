using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Enums;
using UnitTests.Utilities;

namespace UnitTests;

[TestFixture]
public class GameFlowTests
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
    public void PlayersPlaysCardsWithoutSpecials()
    {
        Game game = _game;

        game.StopLaundryTimer();
        game.StopLaundryTurnTimerAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.PlayCard(1, "7", "c");
        game.PlayCard(2, "j", "h");
        StatusMessage statusMessage1 = game.PlayCard(3, "9", "c");

        bool winnerRound1IsCorrect = statusMessage1.Winner.Id == 3 && statusMessage1.Winner == game.CurrentSet.Rounds[0].WinnerStatus.Winner;

        game.PlayCard(3, "q", "d");
        game.PlayCard(1, "a", "d");
        StatusMessage statusMessage2 = game.PlayCard(2, "j", "d");
        
        bool winnerRound2IsCorrect = statusMessage2.Winner.Id == 1 && statusMessage2.Winner == game.CurrentSet.Rounds[1].WinnerStatus.Winner;
        
        game.PlayCard(1, "k", "d");
        game.PlayCard(2, "k", "h");
        StatusMessage statusMessage3 = game.PlayCard(3, "9", "d");
        
        bool winnerRound3IsCorrect = statusMessage3.Winner.Id == 3 && statusMessage3.Winner == game.CurrentSet.Rounds[2].WinnerStatus.Winner;
    
        game.PlayCard(3, "q", "h");
        game.PlayCard(1, "a", "s");
        StatusMessage statusMessage4 = game.PlayCard(2, "a", "h");
        
        //TODO check in current set instead of round
        bool winnerRound4IsCorrect = statusMessage4.Winner.Id == 2 && statusMessage4.Winner == game.CurrentSet.Rounds[3].WinnerStatus.Winner;
    }
}
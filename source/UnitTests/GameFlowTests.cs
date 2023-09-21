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
    public void FirstRoundWithoutSpecials()
    {
        Game game = _game;

        game.StopLaundryTimer();
        game.StopLaundryTurnTimerAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.PlayCard(1, "7", "c");
        game.PlayCard(2, "j", "h");
        StatusMessage statusMessage1 = game.PlayCard(3, "9", "c");

        bool winnerRound1IsCorrect = statusMessage1.Winner.Id == 3 && statusMessage1.Winner == game.Sets[0].Rounds[0].WinnerStatus.Winner;

        game.PlayCard(3, "q", "d");
        game.PlayCard(1, "a", "d");
        StatusMessage statusMessage2 = game.PlayCard(2, "j", "d");
        
        bool winnerRound2IsCorrect = statusMessage2.Winner.Id == 1 && statusMessage2.Winner == game.Sets[0].Rounds[1].WinnerStatus.Winner;
        
        game.PlayCard(1, "k", "d");
        game.PlayCard(2, "k", "h");
        StatusMessage statusMessage3 = game.PlayCard(3, "9", "d");
        
        bool winnerRound3IsCorrect = statusMessage3.Winner.Id == 3 && statusMessage3.Winner == game.Sets[0].Rounds[2].WinnerStatus.Winner;
    
        game.PlayCard(3, "q", "h");
        game.PlayCard(1, "a", "s");
        StatusMessage statusMessage4 = game.PlayCard(2, "a", "h");
        
        bool winnerRound4IsCorrect = statusMessage4.Winner.Id == 2 && statusMessage4.Winner == game.Sets[0].Rounds[3].WinnerStatus.Winner;
        
        Assert.IsTrue(winnerRound1IsCorrect && winnerRound2IsCorrect && winnerRound3IsCorrect && winnerRound4IsCorrect);
    }

    [Test]
    public void PlayWholeSet()
    {
        Game game = _game;

        game.StopLaundryTimer();
        game.StopLaundryTurnTimerAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.PlayCard(1, "7", "c");
        game.PlayCard(2, "j", "h");
        StatusMessage status1Message1 = game.PlayCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = status1Message1.Winner.Id == 3 && status1Message1.Winner == game.Sets[0].Rounds[0].WinnerStatus.Winner;

        game.PlayCard(3, "q", "d");
        game.PlayCard(1, "a", "d");
        StatusMessage status1Message2 = game.PlayCard(2, "j", "d");
        
        bool winnerSet1Round2IsCorrect = status1Message2.Winner.Id == 1 && status1Message2.Winner == game.Sets[0].Rounds[1].WinnerStatus.Winner;
        
        game.PlayCard(1, "k", "d");
        game.PlayCard(2, "k", "h");
        StatusMessage status1Message3 = game.PlayCard(3, "9", "d");
        
        bool winnerSet1Round3IsCorrect = status1Message3.Winner.Id == 3 && status1Message3.Winner == game.Sets[0].Rounds[2].WinnerStatus.Winner;
    
        game.PlayCard(3, "q", "h");
        game.PlayCard(1, "a", "s");
        StatusMessage status1Message4 = game.PlayCard(2, "a", "h");
        
        bool winnerSet1Round4IsCorrect = status1Message4.Winner.Id == 2 && status1Message4.Winner == game.Sets[0].Rounds[3].WinnerStatus.Winner;
        
        
        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];

            if (i == 0)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Hearts, Value.Ten),
                    new(Suit.Spades, Value.Jack),
                    new(Suit.Spades, Value.King),
                    new(Suit.Clubs, Value.Jack),
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 1)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Diamonds, Value.Eight),
                    new(Suit.Spades, Value.Seven),
                    new(Suit.Clubs, Value.Queen),
                    new(Suit.Diamonds, Value.Ten),
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 2)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Spades, Value.Nine),
                    new(Suit.Spades, Value.Eight),
                    new(Suit.Clubs, Value.Ace),
                    new(Suit.Hearts, Value.Nine),
                };
                Entity.SetHandOf(player, cards);
            }
        }
        
        game.StopLaundryTimer();
        game.StopLaundryTurnTimerAndStartRound();
        
        game.PlayCard(3, "a", "c");
        game.PlayCard(1, "j", "c");
        StatusMessage status2Message1 = game.PlayCard(2, "q", "c");

        // TODO: hier gaat het fout
        bool winnerSet2Round1IsCorrect = status2Message1.Winner.Id == 3 && status2Message1.Winner == game.Sets[1].Rounds[0].WinnerStatus.Winner;

        game.PlayCard(3, "q", "d");
        game.PlayCard(1, "a", "d");
        StatusMessage status2Message2 = game.PlayCard(2, "j", "d");
        
        bool winnerSet2Round2IsCorrect = status2Message2.Winner.Id == 1 && status2Message2.Winner == game.Sets[1].Rounds[1].WinnerStatus.Winner;
        
        game.PlayCard(1, "k", "d");
        game.PlayCard(2, "k", "h");
        StatusMessage status2Message3 = game.PlayCard(3, "9", "d");
        
        bool winnerSet2Round3IsCorrect = status2Message3.Winner.Id == 3 && status2Message3.Winner == game.Sets[1].Rounds[2].WinnerStatus.Winner;
    
        game.PlayCard(3, "q", "h");
        game.PlayCard(1, "a", "s");
        StatusMessage status2Message4 = game.PlayCard(2, "a", "h");
        
        bool winnerSet2Round4IsCorrect = status2Message4.Winner.Id == 2 && status2Message4.Winner == game.Sets[1].Rounds[3].WinnerStatus.Winner;
    }
}
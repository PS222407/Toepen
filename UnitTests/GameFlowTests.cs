using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Helpers;
using Toepen_20_BusinessLogicLayer.Models;
using Toepen_20_BusinessLogicLayer.States;
using UnitTests.Utilities;

namespace UnitTests;

//TODO: add test where someone wins set or game when folding

[TestFixture]
public class GameFlowTests
{
    private Game _game;

    [SetUp]
    public void Setup()
    {
        _game = new Game();

        List<Player> players = new()
        {
            new Player("Sam"),
            new Player("Jens"),
            new Player("Mylo")
        };

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            Entity.SetIdOf(player, i + 1);
            _game.AddPlayer(player);
        }

        _game.Start();

        GivePlayerHardCodedSetOfCards();
    }

    private void GivePlayerHardCodedSetOfCards()
    {
        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];

            if (i == 0)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Spades, Value.Ace),
                    new Card(Suit.Diamonds, Value.King),
                    new Card(Suit.Clubs, Value.Seven),
                    new Card(Suit.Diamonds, Value.Ace)
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 1)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Hearts, Value.Jack),
                    new Card(Suit.Hearts, Value.King),
                    new Card(Suit.Diamonds, Value.Jack),
                    new Card(Suit.Hearts, Value.Ace)
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 2)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Clubs, Value.Nine),
                    new Card(Suit.Diamonds, Value.Nine),
                    new Card(Suit.Diamonds, Value.Queen),
                    new Card(Suit.Hearts, Value.Queen)
                };
                Entity.SetHandOf(player, cards);
            }
        }
    }

    [Test]
    public void PlayGameWithoutSpecials_ReturnsWinnersOfRounds()
    {
        Game game = _game;

        // SET 1
        game.BlockLaundryCalls();
        game.BlockLaundryTurnCallsAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.PlayerPlaysCard(1, "7", "c");
        game.PlayerPlaysCard(2, "j", "h");
        game.PlayerPlaysCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = 3 == game.Sets[0].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, "q", "d");
        game.PlayerPlaysCard(1, "a", "d");
        game.PlayerPlaysCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = 1 == game.Sets[0].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, "k", "d");
        game.PlayerPlaysCard(2, "k", "h");
        game.PlayerPlaysCard(3, "9", "d");

        bool winnerSet1Round3IsCorrect = game.Sets[0].Rounds[2].WinnerStatus.Winner.Id == 3;

        game.PlayerPlaysCard(3, "q", "h");
        game.PlayerPlaysCard(1, "a", "s");
        game.PlayerPlaysCard(2, "a", "h");

        bool winnerSet1Round4IsCorrect = game.Sets[0].Rounds[3].WinnerStatus.Winner.Id == 2;
        // sam = 1, jens = 0, mylo = 1


        // SET 2
        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];

            if (i == 0)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Hearts, Value.Ten),
                    new Card(Suit.Spades, Value.Jack),
                    new Card(Suit.Spades, Value.King),
                    new Card(Suit.Clubs, Value.Jack)
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 1)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Diamonds, Value.Eight),
                    new Card(Suit.Spades, Value.Seven),
                    new Card(Suit.Clubs, Value.Queen),
                    new Card(Suit.Diamonds, Value.Ten)
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 2)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Spades, Value.Nine),
                    new Card(Suit.Spades, Value.Eight),
                    new Card(Suit.Clubs, Value.Ace),
                    new Card(Suit.Hearts, Value.Nine)
                };
                Entity.SetHandOf(player, cards);
            }
        }

        game.BlockLaundryCalls();
        game.BlockLaundryTurnCallsAndStartRound();

        game.PlayerPlaysCard(3, "a", "c");
        game.PlayerPlaysCard(1, "j", "c");
        game.PlayerPlaysCard(2, "q", "c");

        bool winnerSet2Round1IsCorrect = 3 == game.Sets[1].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, "9", "h");
        game.PlayerPlaysCard(1, "10", "h");
        game.PlayerPlaysCard(2, "7", "s");

        bool winnerSet2Round2IsCorrect = 1 == game.Sets[1].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, "k", "s");
        game.PlayerPlaysCard(2, "8", "d");
        game.PlayerPlaysCard(3, "9", "s");

        bool winnerSet2Round3IsCorrect = 3 == game.Sets[1].Rounds[2].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, "8", "s");
        game.PlayerPlaysCard(1, "j", "s");
        game.PlayerPlaysCard(2, "10", "d");

        bool winnerSet2Round4IsCorrect = 3 == game.Sets[1].Rounds[3].WinnerStatus.Winner.Id;
        // sam = 2, jens = 1, mylo = 1


        // SET 3
        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];

            if (i == 0)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Hearts, Value.Ace),
                    new Card(Suit.Diamonds, Value.Nine),
                    new Card(Suit.Hearts, Value.King),
                    new Card(Suit.Diamonds, Value.Queen)
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 1)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Hearts, Value.Jack),
                    new Card(Suit.Spades, Value.Eight),
                    new Card(Suit.Diamonds, Value.Ace),
                    new Card(Suit.Hearts, Value.Seven)
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 2)
            {
                List<Card> cards = new()
                {
                    new Card(Suit.Clubs, Value.Seven),
                    new Card(Suit.Clubs, Value.Jack),
                    new Card(Suit.Spades, Value.Jack),
                    new Card(Suit.Spades, Value.Ace)
                };
                Entity.SetHandOf(player, cards);
            }
        }

        game.BlockLaundryCalls();
        game.BlockLaundryTurnCallsAndStartRound();

        game.PlayerPlaysCard(1, "q", "d");
        game.PlayerPlaysCard(2, "a", "d");
        game.PlayerPlaysCard(3, "j", "c");

        bool winnerSet3Round1IsCorrect = 2 == game.Sets[2].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(2, "j", "h");
        game.PlayerPlaysCard(3, "j", "s");
        game.PlayerPlaysCard(1, "k", "h");

        bool winnerSet3Round2IsCorrect = 1 == game.Sets[2].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, "9", "d");
        game.PlayerPlaysCard(2, "7", "h");
        game.PlayerPlaysCard(3, "a", "s");

        bool winnerSet3Round3IsCorrect = 1 == game.Sets[2].Rounds[2].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, "a", "h");
        game.PlayerPlaysCard(2, "8", "s");
        game.PlayerPlaysCard(3, "7", "c");

        bool winnerSet3Round4IsCorrect = 1 == game.Sets[2].Rounds[3].WinnerStatus.Winner.Id;
        // sam = 2, jens = 2, mylo = 2

        // TODO: Add more sets
        Assert.That(winnerSet1Round1IsCorrect &&
                    winnerSet1Round2IsCorrect &&
                    winnerSet1Round3IsCorrect &&
                    winnerSet1Round4IsCorrect &&
                    winnerSet2Round1IsCorrect &&
                    winnerSet2Round2IsCorrect &&
                    winnerSet2Round3IsCorrect &&
                    winnerSet2Round4IsCorrect &&
                    winnerSet3Round1IsCorrect &&
                    winnerSet3Round2IsCorrect &&
                    winnerSet3Round3IsCorrect &&
                    winnerSet3Round4IsCorrect
            , Is.True);
    }

    [Test]
    public void RandomRoundsWithoutSpecials_ReturnsPlayersWithMaxPenaltyPoints()
    {
        Game game = _game;

        Settings.MaxPenaltyPoints = 10000;

        game.BlockLaundryCalls();
        game.BlockLaundryTurnCallsAndStartRound();

        bool gameIsOver = false;
        while (!gameIsOver)
        {
            if (game.CurrentSet.CurrentRound == null)
            {
                game.BlockLaundryCalls();
                game.BlockLaundryTurnCallsAndStartRound();
            }

            Player activePlayer = game.CurrentSet.CurrentRound.ActivePlayer;
            foreach (Card card in new List<Card>(activePlayer.Hand))
            {
                string value = card.Value > Value.Ace ? ((int)card.Value).ToString() : card.Value.ToString().Substring(0, 1);
                string suit = card.Suit.ToString().Substring(0, 1);

                game.PlayerPlaysCard(activePlayer.Id, value, suit);
                if (game.State is GameIsWonAndOver)
                {
                    gameIsOver = true;
                    break;
                }
            }
        }

        int penaltyPointsSam = game.Players[0].PenaltyPoints;
        int penaltyPointsJens = game.Players[1].PenaltyPoints;
        int penaltyPointsMylo = game.Players[2].PenaltyPoints;

        Assert.That(penaltyPointsSam >= Settings.MaxPenaltyPoints || penaltyPointsJens >= Settings.MaxPenaltyPoints || penaltyPointsMylo >= Settings.MaxPenaltyPoints);
    }

    [Test]
    public void Test1SetWithOnlyKnocking_ReturnsPlayersWith5PenaltyPoints()
    {
        // ARRANGE
        Game game = _game;

        // ACT
        game.BlockLaundryCalls();
        game.BlockLaundryTurnCallsAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.Knock(1);
        game.Check(2);
        game.Check(3);
        game.PlayerPlaysCard(1, "7", "c");
        game.PlayerPlaysCard(2, "j", "h");
        game.PlayerPlaysCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = game.Sets[0].Rounds[0].WinnerStatus.Winner.Id == 3;

        game.Knock(3);
        game.Check(1);
        game.Check(2);
        game.PlayerPlaysCard(3, "q", "d");
        game.PlayerPlaysCard(1, "a", "d");
        game.PlayerPlaysCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = game.Sets[0].Rounds[1].WinnerStatus.Winner.Id == 1;

        game.Knock(1);
        game.Check(2);
        game.Check(3);
        game.PlayerPlaysCard(1, "k", "d");
        game.PlayerPlaysCard(2, "k", "h");
        game.PlayerPlaysCard(3, "9", "d");

        bool winnerSet1Round3IsCorrect = game.Sets[0].Rounds[2].WinnerStatus.Winner.Id == 3;

        game.Knock(3);
        game.Check(1);
        game.Check(2);
        game.PlayerPlaysCard(3, "q", "h");
        game.PlayerPlaysCard(1, "a", "s");
        game.PlayerPlaysCard(2, "a", "h");

        bool winnerSet1Round4IsCorrect = 2 == game.Sets[0].Rounds[3].WinnerStatus.Winner.Id;

        // sam = 5, jens = 0, mylo = 5
        // ASSERT
        Assert.That(_game.Players[0].PenaltyPoints == 5 && _game.Players[1].PenaltyPoints == 0 && _game.Players[2].PenaltyPoints == 5);
    }

    /// <summary>
    /// Asserts the penalty points after playing 1 set with players knocking, checking and folding. Midgame the "ruling/winning" player folds so the player next (clockwise) will get the next move, all players do get a penalty point
    /// </summary>
    [Test]
    public void Test1SetWithKnockingAndFoldingAndWinningPlayerFoldsMidGame_ReturnsPlayersWith5PenaltyPoints()
    {
        // ARRANGE
        Game game = _game;

        // ACT
        game.BlockLaundryCalls();
        game.BlockLaundryTurnCallsAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.Knock(1);
        game.Check(2);
        game.Check(3);
        game.PlayerPlaysCard(1, "7", "c");
        game.PlayerPlaysCard(2, "j", "h");
        game.PlayerPlaysCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = 3 == game.Sets[0].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, "q", "d");
        game.PlayerPlaysCard(1, "a", "d");
        game.Knock(2);
        game.Check(3);
        game.Fold(1);
        game.PlayerPlaysCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = 1 == game.Sets[0].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(2, "a", "h");
        game.Knock(3);
        game.Check(2);
        game.PlayerPlaysCard(3, "q", "h");

        bool winnerSet1Round3IsCorrect = 2 == game.Sets[0].Rounds[2].WinnerStatus.Winner.Id;

        game.Knock(2);
        game.Check(3);
        game.PlayerPlaysCard(2, "k", "h");
        game.PlayerPlaysCard(3, "9", "d");

        bool winnerSet1Round4IsCorrect = 2 == game.Sets[0].Rounds[3].WinnerStatus.Winner.Id;

        // sam = 2, jens = 0, mylo = 5
        // ASSERT
        Assert.That(_game.Players[0].PenaltyPoints == 2 && _game.Players[1].PenaltyPoints == 0 && _game.Players[2].PenaltyPoints == 5);
    }

    [Test]
    public void AllPlayersCallLaundry_PlayersGetNewCardsAndPenaltyPoints()
    {
        // ARRANGE
        Game game = _game;

        // ACT
        game.PlayerCallsWhiteLaundry(1);
        game.PlayerCallsDirtyLaundry(2);
        game.PlayerCallsDirtyLaundry(3);
        game.BlockLaundryCalls();
        game.PlayerTurnsLaundry(1, 2);
        game.PlayerTurnsLaundry(1, 3);
        game.PlayerTurnsLaundry(2, 1);
        game.BlockLaundryTurnCallsAndWaitForLaundryCalls();
        game.PlayerCallsDirtyLaundry(3);
        game.BlockLaundryCalls();
        game.PlayerTurnsLaundry(1, 3);
        game.BlockLaundryTurnCallsAndStartRound();

        // ASSERT
        bool penaltyPointsAreCorrect = _game.Players[0].PenaltyPoints == 1 && _game.Players[1].PenaltyPoints == 1 && _game.Players[2].PenaltyPoints == 1;
        Assert.That(penaltyPointsAreCorrect);

        GivePlayerHardCodedSetOfCards();

        Entity.SetActivePlayerOf(game.CurrentSet!.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1)!);
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1)!);

        game.Knock(1);
        game.Check(2);
        game.Check(3);
        game.PlayerPlaysCard(1, "7", "c");
        game.PlayerPlaysCard(2, "j", "h");
        game.PlayerPlaysCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = 3 == game.Sets[0].Rounds[0].WinnerStatus!.Winner.Id;

        game.PlayerPlaysCard(3, "q", "d");
        game.PlayerPlaysCard(1, "a", "d");
        game.Knock(2);
        game.Check(3);
        game.Fold(1);
        game.PlayerPlaysCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = 1 == game.Sets[0].Rounds[1].WinnerStatus!.Winner.Id;

        game.PlayerPlaysCard(2, "a", "h");
        game.Knock(3);
        game.Check(2);
        game.PlayerPlaysCard(3, "q", "h");

        bool winnerSet1Round3IsCorrect = 2 == game.Sets[0].Rounds[2].WinnerStatus!.Winner.Id;

        game.Knock(2);
        game.Check(3);
        game.PlayerPlaysCard(2, "k", "h");
        game.PlayerPlaysCard(3, "9", "d");

        bool winnerSet1Round4IsCorrect = 2 == game.Sets[0].Rounds[3].WinnerStatus!.Winner.Id;

        // sam = 2, jens = 0, mylo = 5
        // ASSERT
        Assert.That(_game.Players[0].PenaltyPoints == 3 && _game.Players[1].PenaltyPoints == 1 && _game.Players[2].PenaltyPoints == 6);
    }
}
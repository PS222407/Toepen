using System.Numerics;
using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
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
        _game = new Game("123");

        List<Player> players = new()
        {
            new Player("Sam"),
            new Player("Jens"),
            new Player("Mylo"),
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
                    new Card(Suit.Diamonds, Value.Ace),
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
                    new Card(Suit.Hearts, Value.Ace),
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
                    new Card(Suit.Hearts, Value.Queen),
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

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.PlayerPlaysCard(1, new Card(Suit.Clubs, Value.Seven));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Jack));
        game.PlayerPlaysCard(3, new Card(Suit.Clubs, Value.Nine));

        bool winnerSet1Round1IsCorrect = 3 == game.Sets[0].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Queen));
        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.Ace));
        game.PlayerPlaysCard(2, new Card(Suit.Diamonds, Value.Jack));

        bool winnerSet1Round2IsCorrect = 1 == game.Sets[0].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.King));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.King));
        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Nine));

        bool winnerSet1Round3IsCorrect = game.Sets[0].Rounds[2].WinnerStatus.Winner.Id == 3;

        game.PlayerPlaysCard(3, new Card(Suit.Hearts, Value.Queen));
        game.PlayerPlaysCard(1, new Card(Suit.Spades, Value.Ace));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Ace));

        bool winnerSet1Round4IsCorrect = game.Sets[0].Rounds[3].WinnerStatus.Winner.Id == 2;
        // sam = 1, jens = 0, mylo = 1
        Player? a = game.GetActivePlayer();

        // Players ready
        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];
            game.PlayerCallsMoveOnToNextSet(player.Id);
        }

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
                    new Card(Suit.Clubs, Value.Jack),
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
                    new Card(Suit.Diamonds, Value.Ten),
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
                    new Card(Suit.Hearts, Value.Nine),
                };
                Entity.SetHandOf(player, cards);
            }
        }

        game.BlockLaundryCalls();

        game.PlayerPlaysCard(3, new Card(Suit.Clubs, Value.Ace));
        game.PlayerPlaysCard(1, new Card(Suit.Clubs, Value.Jack));
        game.PlayerPlaysCard(2, new Card(Suit.Clubs, Value.Queen));

        bool winnerSet2Round1IsCorrect = 3 == game.Sets[1].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, new Card(Suit.Hearts, Value.Nine));
        game.PlayerPlaysCard(1, new Card(Suit.Hearts, Value.Ten));
        game.PlayerPlaysCard(2, new Card(Suit.Spades, Value.Seven));

        bool winnerSet2Round2IsCorrect = 1 == game.Sets[1].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, new Card(Suit.Spades, Value.King));
        game.PlayerPlaysCard(2, new Card(Suit.Diamonds, Value.Eight));
        game.PlayerPlaysCard(3, new Card(Suit.Spades, Value.Nine));

        bool winnerSet2Round3IsCorrect = 3 == game.Sets[1].Rounds[2].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, new Card(Suit.Spades, Value.Eight));
        game.PlayerPlaysCard(1, new Card(Suit.Spades, Value.Jack));
        game.PlayerPlaysCard(2, new Card(Suit.Diamonds, Value.Ten));

        bool winnerSet2Round4IsCorrect = 3 == game.Sets[1].Rounds[3].WinnerStatus.Winner.Id;
        // sam = 2, jens = 1, mylo = 1

        // Players ready
        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];
            game.PlayerCallsMoveOnToNextSet(player.Id);
        }

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
                    new Card(Suit.Diamonds, Value.Queen),
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
                    new Card(Suit.Hearts, Value.Seven),
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
                    new Card(Suit.Spades, Value.Ace),
                };
                Entity.SetHandOf(player, cards);
            }
        }

        game.BlockLaundryCalls();

        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.Queen));
        game.PlayerPlaysCard(2, new Card(Suit.Diamonds, Value.Ace));
        game.PlayerPlaysCard(3, new Card(Suit.Clubs, Value.Jack));

        bool winnerSet3Round1IsCorrect = 2 == game.Sets[2].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Jack));
        game.PlayerPlaysCard(3, new Card(Suit.Spades, Value.Jack));
        game.PlayerPlaysCard(1, new Card(Suit.Hearts, Value.King));

        bool winnerSet3Round2IsCorrect = 1 == game.Sets[2].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.Nine));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Seven));
        game.PlayerPlaysCard(3, new Card(Suit.Spades, Value.Ace));

        bool winnerSet3Round3IsCorrect = 1 == game.Sets[2].Rounds[2].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(1, new Card(Suit.Hearts, Value.Ace));
        game.PlayerPlaysCard(2, new Card(Suit.Spades, Value.Eight));
        game.PlayerPlaysCard(3, new Card(Suit.Clubs, Value.Seven));

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

        Settings.MaxPenaltyPoints = 10;

        game.BlockLaundryCalls();

        bool gameIsOver = false;
        while (!gameIsOver)
        {
            if (game.CurrentSet.CurrentRound == null)
            {
                game.BlockLaundryCalls();
            }

            Player activePlayer = game.CurrentSet.CurrentRound.ActivePlayer;
            foreach (Card card in new List<Card>(activePlayer.Hand))
            {
                try
                {
                    game.PlayerPlaysCard(activePlayer.Id, card);
                }
                catch (CardDoesNotMatchSuitsException)
                {
                }
                catch (NotPlayersTurnException)
                {
                }

                if (game.State is SetIsWonAndOver)
                {
                    for (int i = 0; i < _game.Players.Count; i++)
                    {
                        try
                        {
                            Player player = _game.Players[i];
                            game.PlayerCallsMoveOnToNextSet(player.Id);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                
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

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.PlayerKnocks(1);
        game.PlayerChecks(2);
        game.PlayerChecks(3);
        game.PlayerPlaysCard(1, new Card(Suit.Clubs, Value.Seven));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Jack));
        game.PlayerPlaysCard(3, new Card(Suit.Clubs, Value.Nine));

        bool winnerSet1Round1IsCorrect = game.Sets[0].Rounds[0].WinnerStatus.Winner.Id == 3;

        game.PlayerKnocks(3);
        game.PlayerChecks(1);
        game.PlayerChecks(2);
        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Queen));
        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.Ace));
        game.PlayerPlaysCard(2, new Card(Suit.Diamonds, Value.Jack));

        bool winnerSet1Round2IsCorrect = game.Sets[0].Rounds[1].WinnerStatus.Winner.Id == 1;

        game.PlayerKnocks(1);
        game.PlayerChecks(2);
        game.PlayerChecks(3);
        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.King));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.King));
        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Nine));

        bool winnerSet1Round3IsCorrect = game.Sets[0].Rounds[2].WinnerStatus.Winner.Id == 3;

        game.PlayerKnocks(3);
        game.PlayerChecks(1);
        game.PlayerChecks(2);
        game.PlayerPlaysCard(3, new Card(Suit.Hearts, Value.Queen));
        game.PlayerPlaysCard(1, new Card(Suit.Spades, Value.Ace));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Ace));

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

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        game.PlayerKnocks(1);
        game.PlayerChecks(2);
        game.PlayerChecks(3);
        game.PlayerPlaysCard(1, new Card(Suit.Clubs, Value.Seven));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Jack));
        game.PlayerPlaysCard(3, new Card(Suit.Clubs, Value.Nine));

        bool winnerSet1Round1IsCorrect = 3 == game.Sets[0].Rounds[0].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Queen));
        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.Ace));
        game.PlayerKnocks(2);
        game.PlayerChecks(3);
        game.PlayerFolds(1);
        game.PlayerPlaysCard(2, new Card(Suit.Diamonds, Value.Jack));

        bool winnerSet1Round2IsCorrect = 1 == game.Sets[0].Rounds[1].WinnerStatus.Winner.Id;

        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Ace));
        game.PlayerKnocks(3);
        game.PlayerChecks(2);
        game.PlayerPlaysCard(3, new Card(Suit.Hearts, Value.Queen));

        bool winnerSet1Round3IsCorrect = 2 == game.Sets[0].Rounds[2].WinnerStatus.Winner.Id;

        game.PlayerKnocks(2);
        game.PlayerChecks(3);
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.King));
        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Nine));

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
        // sam = 1, jens = 1, mylo = 1
        game.BlockLaundryTurnCalls();
        game.PlayerCallsDirtyLaundry(3);
        game.BlockLaundryCalls();
        game.PlayerTurnsLaundry(1, 3);
        // sam = 1, jens = 1, mylo = 2
        game.BlockLaundryTurnCalls();

        // ASSERT
        bool penaltyPointsAreCorrect = _game.Players[0].PenaltyPoints == 1 && _game.Players[1].PenaltyPoints == 1 && _game.Players[2].PenaltyPoints == 2;
        Assert.That(penaltyPointsAreCorrect);

        GivePlayerHardCodedSetOfCards();

        Entity.SetActivePlayerOf(game.CurrentSet!.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1)!);
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1)!);

        game.PlayerKnocks(1);
        game.PlayerChecks(2);
        game.PlayerChecks(3);
        game.PlayerPlaysCard(1, new Card(Suit.Clubs, Value.Seven));
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Jack));
        game.PlayerPlaysCard(3, new Card(Suit.Clubs, Value.Nine));

        bool winnerSet1Round1IsCorrect = 3 == game.Sets[0].Rounds[0].WinnerStatus!.Winner.Id;

        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Queen));
        game.PlayerPlaysCard(1, new Card(Suit.Diamonds, Value.Ace));
        game.PlayerKnocks(2);
        game.PlayerChecks(3);
        game.PlayerFolds(1);
        game.PlayerPlaysCard(2, new Card(Suit.Diamonds, Value.Jack));

        bool winnerSet1Round2IsCorrect = 1 == game.Sets[0].Rounds[1].WinnerStatus!.Winner.Id;

        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.Ace));
        game.PlayerKnocks(3);
        game.PlayerChecks(2);
        game.PlayerPlaysCard(3, new Card(Suit.Hearts, Value.Queen));

        bool winnerSet1Round3IsCorrect = 2 == game.Sets[0].Rounds[2].WinnerStatus!.Winner.Id;

        game.PlayerKnocks(2);
        game.PlayerChecks(3);
        game.PlayerPlaysCard(2, new Card(Suit.Hearts, Value.King));
        game.PlayerPlaysCard(3, new Card(Suit.Diamonds, Value.Nine));

        bool winnerSet1Round4IsCorrect = 2 == game.Sets[0].Rounds[3].WinnerStatus!.Winner.Id;

        // ASSERT
        // sam = 3, jens = 1, mylo = 7
        Assert.That(_game.Players[0].PenaltyPoints == 3 && _game.Players[1].PenaltyPoints == 1 && _game.Players[2].PenaltyPoints == 7);
    }
}
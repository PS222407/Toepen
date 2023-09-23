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

        GivePlayerHardCodedSetOfCards();
    }

    private void GivePlayerHardCodedSetOfCards()
    {
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
    public void PlayGameWithoutSpecials_ReturnsWinnersOfRounds()
    {
        Game game = _game;

        // SET 1
        _ = game.StopLaundryTimer();
        _ = game.StopLaundryTurnTimerAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        _ = game.PlayCard(1, "7", "c");
        _ = game.PlayCard(2, "j", "h");
        StatusMessage status1Message1 = game.PlayCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = status1Message1.Winner.Id == 3 && status1Message1.Winner == game.Sets[0].Rounds[0].WinnerStatus.Winner;

        _ = game.PlayCard(3, "q", "d");
        _ = game.PlayCard(1, "a", "d");
        StatusMessage status1Message2 = game.PlayCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = status1Message2.Winner.Id == 1 && status1Message2.Winner == game.Sets[0].Rounds[1].WinnerStatus.Winner;

        _ = game.PlayCard(1, "k", "d");
        _ = game.PlayCard(2, "k", "h");
        StatusMessage status1Message3 = game.PlayCard(3, "9", "d");

        bool winnerSet1Round3IsCorrect = status1Message3.Winner.Id == 3 && status1Message3.Winner == game.Sets[0].Rounds[2].WinnerStatus.Winner;

        _ = game.PlayCard(3, "q", "h");
        _ = game.PlayCard(1, "a", "s");
        StatusMessage status1Message4 = game.PlayCard(2, "a", "h");

        bool winnerSet1Round4IsCorrect = status1Message4.Winner.Id == 2 && status1Message4.Winner == game.Sets[0].Rounds[3].WinnerStatus.Winner;
        // sam = 1, jens = 0, mylo = 1


        // SET 2
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

        _ = game.StopLaundryTimer();
        _ = game.StopLaundryTurnTimerAndStartRound();

        _ = game.PlayCard(3, "a", "c");
        _ = game.PlayCard(1, "j", "c");
        StatusMessage status2Message1 = game.PlayCard(2, "q", "c");

        bool winnerSet2Round1IsCorrect = status2Message1.Winner.Id == 3 && status2Message1.Winner == game.Sets[1].Rounds[0].WinnerStatus.Winner;

        _ = game.PlayCard(3, "9", "h");
        _ = game.PlayCard(1, "10", "h");
        StatusMessage status2Message2 = game.PlayCard(2, "7", "s");

        bool winnerSet2Round2IsCorrect = status2Message2.Winner.Id == 1 && status2Message2.Winner == game.Sets[1].Rounds[1].WinnerStatus.Winner;

        _ = game.PlayCard(1, "k", "s");
        _ = game.PlayCard(2, "8", "d");
        StatusMessage status2Message3 = game.PlayCard(3, "9", "s");

        bool winnerSet2Round3IsCorrect = status2Message3.Winner.Id == 3 && status2Message3.Winner == game.Sets[1].Rounds[2].WinnerStatus.Winner;

        _ = game.PlayCard(3, "8", "s");
        _ = game.PlayCard(1, "j", "s");
        StatusMessage status2Message4 = game.PlayCard(2, "10", "d");

        bool winnerSet2Round4IsCorrect = status2Message4.Winner.Id == 3 && status2Message4.Winner == game.Sets[1].Rounds[3].WinnerStatus.Winner;
        // sam = 2, jens = 1, mylo = 1


        // SET 3
        for (int i = 0; i < _game.Players.Count; i++)
        {
            Player player = _game.Players[i];

            if (i == 0)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Hearts, Value.Ace),
                    new(Suit.Diamonds, Value.Nine),
                    new(Suit.Hearts, Value.King),
                    new(Suit.Diamonds, Value.Queen),
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 1)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Hearts, Value.Jack),
                    new(Suit.Spades, Value.Eight),
                    new(Suit.Diamonds, Value.Ace),
                    new(Suit.Hearts, Value.Seven),
                };
                Entity.SetHandOf(player, cards);
            }

            if (i == 2)
            {
                List<Card> cards = new List<Card>
                {
                    new(Suit.Clubs, Value.Seven),
                    new(Suit.Clubs, Value.Jack),
                    new(Suit.Spades, Value.Jack),
                    new(Suit.Spades, Value.Ace),
                };
                Entity.SetHandOf(player, cards);
            }
        }

        _ = game.StopLaundryTimer();
        _ = game.StopLaundryTurnTimerAndStartRound();

        _ = game.PlayCard(1, "q", "d");
        _ = game.PlayCard(2, "a", "d");
        StatusMessage status3Message1 = game.PlayCard(3, "j", "c");

        bool winnerSet3Round1IsCorrect = status3Message1.Winner.Id == 2 && status3Message1.Winner == game.Sets[2].Rounds[0].WinnerStatus.Winner;

        _ = game.PlayCard(2, "j", "h");
        _ = game.PlayCard(3, "j", "s");
        StatusMessage status3Message2 = game.PlayCard(1, "k", "h");

        bool winnerSet3Round2IsCorrect = status3Message2.Winner.Id == 1 && status3Message2.Winner == game.Sets[2].Rounds[1].WinnerStatus.Winner;

        _ = game.PlayCard(1, "9", "d");
        _ = game.PlayCard(2, "7", "h");
        StatusMessage status3Message3 = game.PlayCard(3, "a", "s");

        bool winnerSet3Round3IsCorrect = status3Message3.Winner.Id == 1 && status3Message3.Winner == game.Sets[2].Rounds[2].WinnerStatus.Winner;

        _ = game.PlayCard(1, "a", "h");
        _ = game.PlayCard(2, "8", "s");
        StatusMessage status3Message4 = game.PlayCard(3, "7", "c");

        bool winnerSet3Round4IsCorrect = status3Message4.Winner.Id == 1 && status3Message4.Winner == game.Sets[2].Rounds[3].WinnerStatus.Winner;
        // sam = 2, jens = 2, mylo = 2

        // TODO: Add more sets
        Assert.IsTrue(winnerSet1Round1IsCorrect &&
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
        );
    }

    [Test]
    public void RandomRoundsWithoutSpecials_ReturnsPlayersWithMaxPenaltyPoints()
    {
        Game game = _game;

        Settings.MaxPenaltyPoints = 10000;

        _ = game.StopLaundryTimer();
        _ = game.StopLaundryTurnTimerAndStartRound();

        bool gameIsOver = false;
        while (!gameIsOver)
        {
            if (game.CurrentSet.CurrentRound == null)
            {
                _ = game.StopLaundryTimer();
                _ = game.StopLaundryTurnTimerAndStartRound();
            }

            Player activePlayer = game.CurrentSet.CurrentRound.ActivePlayer;
            foreach (Card card in new List<Card>(activePlayer.Hand))
            {
                string value = card.Value > Value.Ace ? ((int)card.Value).ToString() : card.Value.ToString().Substring(0, 1);
                string suit = card.Suit.ToString().Substring(0, 1);

                StatusMessage statusMessage = game.PlayCard(activePlayer.Id, value, suit);
                if (statusMessage.Success)
                {
                    gameIsOver = statusMessage.Message == Message.APlayerHasWonGame;
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
        _ = game.StopLaundryTimer();
        _ = game.StopLaundryTurnTimerAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        _ = game.Knock(1);
        _ = game.Check(2);
        _ = game.Check(3);
        _ = game.PlayCard(1, "7", "c");
        _ = game.PlayCard(2, "j", "h");
        StatusMessage status1Message1 = game.PlayCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = status1Message1.Winner.Id == 3 && status1Message1.Winner == game.Sets[0].Rounds[0].WinnerStatus.Winner;

        _ = game.Knock(3);
        _ = game.Check(1);
        _ = game.Check(2);
        _ = game.PlayCard(3, "q", "d");
        _ = game.PlayCard(1, "a", "d");
        StatusMessage status1Message2 = game.PlayCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = status1Message2.Winner.Id == 1 && status1Message2.Winner == game.Sets[0].Rounds[1].WinnerStatus.Winner;

        _ = game.Knock(1);
        _ = game.Check(2);
        _ = game.Check(3);
        _ = game.PlayCard(1, "k", "d");
        _ = game.PlayCard(2, "k", "h");
        StatusMessage status1Message3 = game.PlayCard(3, "9", "d");

        bool winnerSet1Round3IsCorrect = status1Message3.Winner.Id == 3 && status1Message3.Winner == game.Sets[0].Rounds[2].WinnerStatus.Winner;

        _ = game.Knock(3);
        _ = game.Check(1);
        _ = game.Check(2);
        _ = game.PlayCard(3, "q", "h");
        _ = game.PlayCard(1, "a", "s");
        StatusMessage status1Message4 = game.PlayCard(2, "a", "h");

        bool winnerSet1Round4IsCorrect = status1Message4.Winner.Id == 2 && status1Message4.Winner == game.Sets[0].Rounds[3].WinnerStatus.Winner;

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
        _ = game.StopLaundryTimer();
        _ = game.StopLaundryTurnTimerAndStartRound();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        _ = game.Knock(1);
        _ = game.Check(2);
        _ = game.Check(3);
        _ = game.PlayCard(1, "7", "c");
        _ = game.PlayCard(2, "j", "h");
        StatusMessage status1Message1 = game.PlayCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = status1Message1.Winner.Id == 3 && status1Message1.Winner == game.Sets[0].Rounds[0].WinnerStatus.Winner;

        _ = game.PlayCard(3, "q", "d");
        _ = game.PlayCard(1, "a", "d");
        _ = game.Knock(2);
        _ = game.Check(3);
        _ = game.Fold(1);
        StatusMessage status1Message2 = game.PlayCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = status1Message2.Winner.Id == 1 && status1Message2.Winner == game.Sets[0].Rounds[1].WinnerStatus.Winner;

        _ = game.PlayCard(2, "a", "h");
        _ = game.Knock(3);
        _ = game.Check(2);
        StatusMessage status1Message3 = game.PlayCard(3, "q", "h");

        bool winnerSet1Round3IsCorrect = status1Message3.Winner.Id == 2 && status1Message3.Winner == game.Sets[0].Rounds[2].WinnerStatus.Winner;

        _ = game.Knock(2);
        _ = game.Check(3);
        _ = game.PlayCard(2, "k", "h");
        StatusMessage status1Message4 = game.PlayCard(3, "9", "d");

        bool winnerSet1Round4IsCorrect = status1Message4.Winner.Id == 2 && status1Message4.Winner == game.Sets[0].Rounds[3].WinnerStatus.Winner;

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
        _ = game.WhiteLaundry(1);
        _ = game.DirtyLaundry(2);
        _ = game.DirtyLaundry(3);
        _ = game.StopLaundryTimer();
        _ = game.TurnsLaundry(1, 2);
        _ = game.TurnsLaundry(1, 3);
        _ = game.TurnsLaundry(2, 1);
        _ = game.StopLaundryTurnTimerAndStartLaundryTimer();
        StatusMessage statusMessageAlreadyCalledLaundry = game.DirtyLaundry(3);
        _ = game.StopLaundryTimer();
        StatusMessage statusMessageAlreadyBeenTurned = game.TurnsLaundry(1, 3);
        _ = game.StopLaundryTurnTimerAndStartRound();

        // ASSERT
        bool penaltyPointsAreCorrect = _game.Players[0].PenaltyPoints == 1 && _game.Players[1].PenaltyPoints == 1 && _game.Players[2].PenaltyPoints == 1;
        Assert.That(penaltyPointsAreCorrect && statusMessageAlreadyBeenTurned.Message == Message.AlreadyTurnedLaundry && statusMessageAlreadyCalledLaundry.Message == Message.AlreadyCalledLaundry);

        GivePlayerHardCodedSetOfCards();

        Entity.SetActivePlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));
        Entity.SetStartedPlayerOf(game.CurrentSet.CurrentRound, game.CurrentSet.CurrentRound.Players.Find(p => p.Id == 1));

        _ = game.Knock(1);
        _ = game.Check(2);
        _ = game.Check(3);
        _ = game.PlayCard(1, "7", "c");
        _ = game.PlayCard(2, "j", "h");
        StatusMessage status1Message1 = game.PlayCard(3, "9", "c");

        bool winnerSet1Round1IsCorrect = status1Message1.Winner.Id == 3 && status1Message1.Winner == game.Sets[0].Rounds[0].WinnerStatus.Winner;

        _ = game.PlayCard(3, "q", "d");
        _ = game.PlayCard(1, "a", "d");
        _ = game.Knock(2);
        _ = game.Check(3);
        _ = game.Fold(1);
        StatusMessage status1Message2 = game.PlayCard(2, "j", "d");

        bool winnerSet1Round2IsCorrect = status1Message2.Winner.Id == 1 && status1Message2.Winner == game.Sets[0].Rounds[1].WinnerStatus.Winner;

        _ = game.PlayCard(2, "a", "h");
        _ = game.Knock(3);
        _ = game.Check(2);
        StatusMessage status1Message3 = game.PlayCard(3, "q", "h");

        bool winnerSet1Round3IsCorrect = status1Message3.Winner.Id == 2 && status1Message3.Winner == game.Sets[0].Rounds[2].WinnerStatus.Winner;

        _ = game.Knock(2);
        _ = game.Check(3);
        _ = game.PlayCard(2, "k", "h");
        StatusMessage status1Message4 = game.PlayCard(3, "9", "d");

        bool winnerSet1Round4IsCorrect = status1Message4.Winner.Id == 2 && status1Message4.Winner == game.Sets[0].Rounds[3].WinnerStatus.Winner;

        // sam = 2, jens = 0, mylo = 5
        // ASSERT
        Assert.That(_game.Players[0].PenaltyPoints == 3 && _game.Players[1].PenaltyPoints == 1 && _game.Players[2].PenaltyPoints == 6);
    }
}
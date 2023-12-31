﻿using System.Text.RegularExpressions;
using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.States;

public class Initialized : IState
{
    public void AddPlayer(Game game, Player player)
    {
        if (player.IsOutOfGame())
        {
            throw new PlayerIsOutOfGameException();
        }

        if (game.Players.Count >= Game.MaxAmountOfPlayers)
        {
            throw new TooManyPlayersException();
        }

        if (string.IsNullOrWhiteSpace(player.Name))
        {
            throw new PlayerEmptyUserName();
        }

        if (game.Players.Any(p => string.Equals(Regex.Replace(p.Name, @"\s", ""), Regex.Replace(player.Name, @"\s", ""), StringComparison.OrdinalIgnoreCase)))
        {
            throw new PlayerAlreadyExistsException();
        }

        game.Players.Add(player);
    }

    public void RemovePlayer(Game game, Player player)
    {
        game.Players.Remove(player);
    }

    public void Start(Game game)
    {
        if (game.Players.Count < Game.MinAmountOfPlayer)
        {
            throw new NotEnoughPlayersException();
        }

        game.State = new WaitingForLaundryCalls();
        game.StartNewSet();
    }

    public void PlayerCallsDirtyLaundry(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerCallsWhiteLaundry(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerCallsNoLaundry(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public Message PlayerTurnsLaundry(Game game, Player player, Player victim)
    {
        throw new InvalidStateException();
    }

    public void BlockLaundryTurnCalls(Game game)
    {
        throw new InvalidStateException();
    }

    public void BlockLaundryCalls(Game game)
    {
        throw new InvalidStateException();
    }

    public TimerInfo LaundryTimerCallback(Game game)
    {
        throw new InvalidStateException();
    }

    public TimerInfo LaundryTurnTimerCallback(Game game)
    {
        throw new InvalidStateException();
    }

    public void PlayerKnocks(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerChecks(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerFolds(Game game, Player player)
    {
        throw new InvalidStateException();
    }

    public void PlayerPlaysCard(Game game, Player player, Card card)
    {
        throw new InvalidStateException();
    }

    public Player GetWinner(Game game)
    {
        throw new InvalidStateException();
    }

    public void StartNewSet(Game game)
    {
        throw new InvalidStateException();
    }

    public void PlayerMovesOnToNextSet(Game game, Player player)
    {
        throw new InvalidStateException();
    }
}
using System.Numerics;
using Toepen_10_Hub.ViewModels;
using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Services;

public class GameTransformer
{
    public static GameViewModel GameToViewModel(Game game, string connectionId)
    {
        GameViewModel gameViewModel = new();

        List<PlayerViewModel> playerViewModels = new();
        foreach (Player player in game.Players)
        {
            List<CardViewModel> cardViewModels = new();
            cardViewModels.AddRange(player.ConnectionId == connectionId || player.PlayWithOpenCards
                ? player.Hand.Select(card => new CardViewModel { Suit = card.Suit.ToString(), Value = card.Value.ToString() })
                : player.Hand.Select(_ => new CardViewModel { Suit = "x", Value = "X" }));
            
            CardViewModel? lastPlayedCardViewModel = player.PlayedCards.Count > 0 ? new CardViewModel
            {
                Suit = player.PlayedCards.Last().Suit.ToString(),
                Value = player.PlayedCards.Last().Value.ToString()
            } : null;

            PlayerViewModel playerViewModel = new()
            {
                Id = player.Id,
                Name = player.Name,
                IsHost = player.IsHost,
                IsYou = player.ConnectionId == connectionId,
                IsActive = game.GetActivePlayer()?.Id == player.Id,
                HasFolded = player.HasFolded,
                PenaltyPoints = player.PenaltyPoints,
                HasKnocked = game.GetPlayerWhoKnocked()?.Id == player.Id,
                CalledDirtyLaundry = player.HasCalledDirtyLaundry,
                CalledWhiteLaundry = player.HasCalledWhiteLaundry,
                Hand = cardViewModels,
                LastPlayedCard = lastPlayedCardViewModel,
            };
            playerViewModels.Add(playerViewModel);
        }

        CardViewModel? startedCard = game.CurrentSet?.CurrentRound?.StartedCard != null ? new CardViewModel
        {
            Suit = game.CurrentSet.CurrentRound.StartedCard.Suit.ToString(),
            Value = game.CurrentSet.CurrentRound.StartedCard.Value.ToString()
        } : null;

        gameViewModel.StartedCard = startedCard;
        gameViewModel.State = game.State.GetType().ToString().Substring(game.State.GetType().ToString().LastIndexOf('.') + 1);
        gameViewModel.Players = playerViewModels;
        gameViewModel.SetNumber = game.Sets.Count;
        gameViewModel.RoundNumber = game.CurrentSet?.Rounds.Count ?? 0;
        gameViewModel.PenaltyPoints = game.CurrentSet?.PenaltyPoints ?? 0;
        gameViewModel.WinnerIdOfSet = game.CurrentSet?.WinnerOfSet?.Id ?? -1;
        gameViewModel.WinnerIdOfGame = game.GetWinner()?.Id ?? -1;

        SetUserOrder(gameViewModel);
        
        return gameViewModel;
    }
    
    public static PlayerViewModel PlayerToViewModel(Player player, string connectionId)
    {
        PlayerViewModel playerViewModel = new PlayerViewModel
        {
            Id = player.Id,
            Name = player.Name,
            IsHost = player.IsHost,
            IsYou = player.ConnectionId == connectionId,
        };

        return playerViewModel;
    }

    public static TurnLaundryViewModel PlayerCardsToViewModel(Player player, Player victim)
    {
        List<CardViewModel> cardViewModels = victim.Hand.Select(card => new CardViewModel { Suit = card.Suit.ToString(), Value = card.Value.ToString() }).ToList();

        return new TurnLaundryViewModel
        {
            PlayerName = player.Name,
            VictimName = victim.Name,
            Hand = cardViewModels,
        };
    }

    private static void SetUserOrder(GameViewModel gameViewModel)
    {
        int playersIndex = gameViewModel.Players.FindIndex(user => user.IsYou);

        if (playersIndex != -1)
        {
            List<PlayerViewModel> firstPart = gameViewModel.Players.GetRange(playersIndex, gameViewModel.Players.Count - playersIndex);
            List<PlayerViewModel> secondPart = gameViewModel.Players.GetRange(0, playersIndex);

            gameViewModel.Players.Clear();

            gameViewModel.Players.AddRange(firstPart);
            gameViewModel.Players.AddRange(secondPart);
        }
    }

    /// <exception cref="CardNotFoundException"></exception>
    public static Card CardViewModelToCard(CardViewModel cardViewModel)
    {
        try
        {
            return new Card(Enum.Parse<Suit>(cardViewModel.Suit), Enum.Parse<Value>(cardViewModel.Value));
        }
        catch (ArgumentException)
        {
            throw new CardNotFoundException();
        }
    }
}
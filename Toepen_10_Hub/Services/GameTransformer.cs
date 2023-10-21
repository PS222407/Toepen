using Toepen_10_Hub.ViewModels;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Services;

public class GameTransformer
{
    public GameViewModel GameToViewModel(Game game, string connectionId)
    {
        GameViewModel gameViewModel = new();

        List<PlayerViewModel> playerViewModels = new();
        foreach (Player player in game.Players)
        {
            // TODO: player play with open cards
            List<CardViewModel> cardViewModels = new();
            cardViewModels.AddRange(player.ConnectionId == connectionId
                ? player.Hand.Select(card => new CardViewModel { Suit = card.Suit.ToString(), Value = card.Value.ToString() })
                : player.Hand.Select(_ => new CardViewModel { Suit = "x", Value = "X" }));

            PlayerViewModel playerViewModel = new()
            {
                Name = player.Name,
                IsYou = player.ConnectionId == connectionId,
                IsActive = game.CurrentSet?.CurrentRound?.ActivePlayer?.ConnectionId == connectionId,
                HasKnocked = game.CurrentSet?.CurrentRound?.PlayerWhoKnocked?.ConnectionId == connectionId,
                CalledDirtyLaundry = player.HasCalledDirtyLaundry,
                CalledWhiteLaundry = player.HasCalledWhiteLaundry,
                Hand = cardViewModels
            };
            playerViewModels.Add(playerViewModel);
        }

        gameViewModel.Players = playerViewModels;
        gameViewModel.SetNumber = game.Sets.Count;
        gameViewModel.RoundNumber = game.CurrentSet?.Rounds.Count ?? 0;
        gameViewModel.PenaltyPoints = game.CurrentSet?.PenaltyPoints ?? 0;

        return gameViewModel;
    }
}
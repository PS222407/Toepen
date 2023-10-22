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

            PlayerViewModel playerViewModel = new()
            {
                Id = player.Id,
                Name = player.Name,
                IsYou = player.ConnectionId == connectionId,
                IsActive = game.GetActivePlayer()?.ConnectionId == connectionId,
                HasKnocked = game.GetPlayerWhoKnocked()?.ConnectionId == connectionId,
                CalledDirtyLaundry = player.HasCalledDirtyLaundry,
                CalledWhiteLaundry = player.HasCalledWhiteLaundry,
                Hand = cardViewModels,
            };
            playerViewModels.Add(playerViewModel);
        }

        gameViewModel.Players = playerViewModels;
        gameViewModel.SetNumber = game.Sets.Count;
        gameViewModel.RoundNumber = game.CurrentSet?.Rounds.Count ?? 0;
        gameViewModel.PenaltyPoints = game.CurrentSet?.PenaltyPoints ?? 0;

        return gameViewModel;
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
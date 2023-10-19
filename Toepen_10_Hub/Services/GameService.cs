using Toepen_10_Hub.Interfaces;
using Toepen_10_Hub.ViewModels;

namespace Toepen_10_Hub.Services;

public class GameService : IGameService
{
    public List<GameViewModel> Games { get; set; } = new();
}
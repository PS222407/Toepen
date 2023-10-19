using Toepen_10_Hub.ViewModels;

namespace Toepen_10_Hub.Interfaces;

public interface IGameService
{
    public List<GameViewModel> Games { get; set; }
}
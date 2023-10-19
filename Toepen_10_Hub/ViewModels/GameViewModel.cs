namespace Toepen_10_Hub.ViewModels;

public class GameViewModel()
{
    public string RoomCode { get; set; }

    public List<PlayerViewModel> Players { get; set; } = new();
}
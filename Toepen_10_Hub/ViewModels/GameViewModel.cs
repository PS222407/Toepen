namespace Toepen_10_Hub.ViewModels;

public class GameViewModel
{
    public string State { get; set; }

    public int WinnerIdOfSet { get; set; }

    public int WinnerIdOfGame { get; set; }

    public int PenaltyPoints { get; set; }

    public int SetNumber { get; set; }

    public int RoundNumber { get; set; }

    public List<PlayerViewModel> Players { get; set; } = new();
}
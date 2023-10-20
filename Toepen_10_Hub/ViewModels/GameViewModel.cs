namespace Toepen_10_Hub.ViewModels;

public class GameViewModel
{
    public int PenaltyPoints { get; set; }

    public int SetNumber { get; set; }

    public int RoundNumber { get; set; }
    
    public List<PlayerViewModel> Players { get; set; } = new();
}
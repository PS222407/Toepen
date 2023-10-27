namespace Toepen_10_Hub.ViewModels;

public class TurnLaundryViewModel
{
    public string PlayerName { get; set; }

    public string VictimName { get; set; }

    public List<CardViewModel> Hand { get; set; } = new();

    public bool victimBluffed { get; set; }
}

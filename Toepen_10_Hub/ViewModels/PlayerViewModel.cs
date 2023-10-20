namespace Toepen_10_Hub.ViewModels;

public class PlayerViewModel
{
    public string Name { get; set; }

    public bool IsYou { get; set; }

    public bool IsActive { get; set; }

    public bool HasKnocked { get; set; }

    public bool CalledWhiteLaundry { get; set; }

    public bool CalledDirtyLaundry { get; set; }

    public List<CardViewModel> Hand { get; set; } = new();
}
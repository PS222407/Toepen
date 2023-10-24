namespace Toepen_10_Hub.ViewModels;

public class PlayerViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public bool IsHost { get; set; }

    public bool IsYou { get; set; }

    public bool IsActive { get; set; }
    
    public bool HasFolded { get; set; }

    public int PenaltyPoints { get; set; }

    public bool HasKnocked { get; set; }

    public bool CalledWhiteLaundry { get; set; }

    public bool CalledDirtyLaundry { get; set; }

    public List<CardViewModel> Hand { get; set; } = new();

    public CardViewModel? LastPlayedCard { get; set; } = new();
}
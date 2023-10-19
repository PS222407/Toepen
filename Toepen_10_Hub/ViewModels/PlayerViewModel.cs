namespace Toepen_10_Hub.ViewModels;

public class PlayerViewModel()
{
    public string Id { get; set; }
    
    public string Name { get; set; }

    public List<CardViewModel> CardViewModels { get; set; } = new();
}
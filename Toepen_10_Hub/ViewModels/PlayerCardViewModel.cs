namespace Toepen_10_Hub.ViewModels
{
    public class PlayerCardViewModel
    {
        public string PlayerName { get; set; }
        public string VictimName { get; set; }
        public List<CardViewModel> Hand { get; set; } = new();
    }
}

using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Helpers;

public class WinnerStatus
{
    public Player Winner { get; set; }

    public bool WinnerOfSet { get; set; }

    public int RoundNumber { get; set; }
}
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_20_BusinessLogicLayer.Helpers;

public class WinnerStatus
{
    public Player Winner { get; set; }

    public bool WinnerOfSet { get; set; }

    public int RoundNumber { get; set; }
}
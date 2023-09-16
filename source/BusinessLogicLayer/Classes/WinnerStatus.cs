namespace BusinessLogicLayer.Classes;

public class WinnerStatus
{
    public Player Winner { get; private set; }

    public bool WinnerOfSet { get; private set; }

    public WinnerStatus(Player winner, bool winnerOfSet)
    {
        Winner = winner;
        WinnerOfSet = winnerOfSet;
    }
}
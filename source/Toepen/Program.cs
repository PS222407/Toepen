// See https://aka.ms/new-console-template for more information

using BusinessLogicLayer.Classes;

Game game = new Game();

if (!game.AddPlayer(new Player("Niels")) ||
    !game.AddPlayer(new Player("Mylo")) ||
    !game.AddPlayer(new Player("Jens")) ||
    !game.AddPlayer(new Player("Sam")) ||
    !game.AddPlayer(new Player("Bas")) ||
    !game.AddPlayer(new Player("Gijs"))
   )
{
    Console.WriteLine("Too many players! Max 6");
    return;
}

game.Start();

// foreach (Card card in game.Deck)
// {
//     Console.WriteLine(card);
// }
foreach (Player player in game.Players)
{
    Console.WriteLine("--------------------------");
    Console.WriteLine($"{player.Id} {player.Name}");
    foreach (Card card in player.Hand)
    {
        Console.WriteLine(card);
    }

    if (player.HasDirtyLaundry())
    {
        Console.WriteLine("Heeft vuile was");
    }

    if (player.HasWhiteLaundry())
    {
        Console.WriteLine("Heeft witte was");
    }
}
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

// =========================================================
// readline
// =========================================================
Console.WriteLine("Available commands:");
Console.WriteLine("---------------------");
Console.WriteLine("vuilewas");
Console.WriteLine("wittewas");
Console.WriteLine("turnslaundry");
Console.WriteLine("check");
Console.WriteLine("fold");
Console.WriteLine("knock");
Console.WriteLine("---------------------");
string? command = Console.ReadLine();

switch (command)
{
    case "vuilewas":
        game.DirtyLaundry(1);
        break;
    case "wittewas":
        game.WhiteLaundry(1);
        break;
    case "turnslaundry":
        game.TurnsLaundry(1);
        break;
    case "check":
        game.Check(1);
        break;
    case "fold":
        game.Fold(1);
        break;
    case "knock":
        game.Knock(1);
        break;
    default:
        Console.WriteLine("Incorrect commands:");
        Console.WriteLine("---------------------");
        Console.WriteLine("vuilewas");
        Console.WriteLine("wittewas");
        Console.WriteLine("turnslaundry");
        Console.WriteLine("check");
        Console.WriteLine("fold");
        Console.WriteLine("knock");
        Console.WriteLine("---------------------");
        break;
}
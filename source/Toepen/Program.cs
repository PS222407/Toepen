using System.Diagnostics;
using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Enums;

Game game = new Game();

// =========================================================
// readline
// =========================================================
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("---------------------");
Console.WriteLine("Available commands:");
Console.WriteLine("---------------------");
foreach (Commands command in Enum.GetValues(typeof(Commands)))
{
    Console.WriteLine(command);
}
Console.WriteLine("---------------------");
while (true)
{
    string? command = Console.ReadLine();
    if (command == null)
    {
        return;
    }
    string[] inputParts = command.Split(' ');
    Enum.TryParse(typeof(Commands), inputParts[0], out object? inputCommand);

    switch (inputCommand)
    {
        case Commands.Start:
        {
            if (!game.Start())
            {
                Console.WriteLine();
                Console.WriteLine("---------------------");
                Console.WriteLine("Warning: the game already started");
                Console.WriteLine("---------------------");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("---------------------");
                Console.WriteLine("GAME STARTED");
                Console.WriteLine("---------------------");

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
            }

            break;
        }
        case Commands.AddPlayer:
        {
            if (inputParts.Length != 2)
            {
                Console.WriteLine($"Wrong command, please do: {Commands.AddPlayer.ToString()} playername");
            }
            else if (!game.AddPlayer(new Player(inputParts[1])))
            {
                Console.WriteLine("Lobby is either full or the game has been started!");
            }

            break;
        }
        case Commands.DirtyLaundry:
            game.DirtyLaundry(1);
            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine("Player 1 zegt vuilewas te hebben");
            Console.WriteLine("---------------------");
            break;
        case Commands.WhiteLaundry:
            game.WhiteLaundry(1);
            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine("Player 1 zegt wittewas te hebben");
            Console.WriteLine("---------------------");
            break;
        case Commands.StopLaundryTimer:
            game.LaundryTimeIsUp();
            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine("Laundry timer stopped");
            Console.WriteLine("---------------------");
            break;
        case Commands.TurnsLaundry:
            Thread.Sleep(1000);
            Console.WriteLine(1);
            Thread.Sleep(1000);
            Console.WriteLine(2);
            Thread.Sleep(1000);
            Console.WriteLine(3);
            Thread.Sleep(1000);
            Console.WriteLine(4);
            Thread.Sleep(1000);
            Console.WriteLine(5);

            //TODO:
            // game.TurnsLaundry(2);
            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine("Player 2 draait de was om");
            Console.WriteLine("---------------------");
            break;
        case Commands.Check:
            game.Check(2);
            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine("Player 2 checkt");
            Console.WriteLine("---------------------");
            break;
        case Commands.Fold:
            game.Fold(2);
            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine("Player 2 fold");
            Console.WriteLine("---------------------");
            break;
        case Commands.Knock:
            game.Knock(1);
            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine("Player 1 klopt");
            Console.WriteLine("---------------------");
            break;
        default:
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Incorrect! List of available commands:");
            Console.WriteLine("---------------------");
            foreach (Commands cmd in Enum.GetValues(typeof(Commands)))
            {
                Console.WriteLine(cmd);
            }
            Console.WriteLine("---------------------");
            break;
    }
}
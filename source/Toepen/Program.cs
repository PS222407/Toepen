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
            Start();
            break;
        case Commands.AddPlayer:
            AddPlayer(inputParts);
            break;
        case Commands.DirtyLaundry:
            DirtyLaundry(inputParts);
            break;
        case Commands.WhiteLaundry:
            WhiteLaundry(inputParts);
            break;
        case Commands.StopLaundryTimer:
            StopLaundryTimer();
            break;
        case Commands.TurnsLaundry:
            TurnsLaundry(inputParts);
            break;
        case Commands.Check:
            Check(inputParts);
            break;
        case Commands.Fold:
            Fold(inputParts);
            break;
        case Commands.Knock:
            Knock(inputParts);
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

void Start()
{
    StatusMessage statusMessage = game.Start();
    if (!statusMessage.Success)
    {
        Console.WriteLine();
        Console.WriteLine("---------------------");
        Console.WriteLine($"Warning: {statusMessage.Message}");
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
}

void AddPlayer(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Commands.AddPlayer.ToString()} playername");
    }
    else if (!game.AddPlayer(new Player(args[1])))
    {
        Console.WriteLine("Lobby is either full or the game has been started!");
    }
}

void DirtyLaundry(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Commands.DirtyLaundry.ToString()} playerid");
        return;
    }
    StatusMessage statusMessage = game.DirtyLaundry(int.Parse(args[1]));
    if (!statusMessage.Success)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} zegt vuilewas te hebben");
    Console.WriteLine("---------------------");
}

void WhiteLaundry(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Commands.WhiteLaundry.ToString()} playerid");
        return;
    }
    if (!game.WhiteLaundry(int.Parse(args[1])))
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} zegt wittewas te hebben");
    Console.WriteLine("---------------------");
}

void StopLaundryTimer()
{
    if (!game.StopLaundryTimer())
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine("Laundry timer stopped");
    Console.WriteLine("---------------------");
}

void TurnsLaundry(string[] args)
{
    if (args.Length != 3)
    {
        Console.WriteLine($"Wrong command, please do: {Commands.TurnsLaundry.ToString()} turnerId victimId");
        return;
    }

    StatusMessage statusMessage = game.TurnsLaundry(int.Parse(args[1]), int.Parse(args[2]));
    if (!statusMessage.Success)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} draait de was om van speler {args[2]}");
    Console.WriteLine("---------------------");

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"{args[2]} {statusMessage.Message}");
    Console.WriteLine("---------------------");
}

void Check(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Commands.Check.ToString()} playerid");
        return;
    }
    
    game.Check(int.Parse(args[1]));
    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} checkt");
    Console.WriteLine("---------------------");
}

void Fold(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Commands.Fold.ToString()} playerid");
        return;
    }
    
    game.Fold(int.Parse(args[1]));
    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} fold");
    Console.WriteLine("---------------------");
}

void Knock(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Commands.Knock.ToString()} playerid");
        return;
    }
    
    game.Knock(int.Parse(args[1]));
    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} klopt");
    Console.WriteLine("---------------------");
}
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
foreach (Command command in Enum.GetValues(typeof(Command)))
{
    Console.WriteLine(command);
}

Console.WriteLine("---------------------");

// TODO: remove these test scenario's
AddPlayer(new[] { "jens", "jens" });
AddPlayer(new[] { "sam", "sam" });
AddPlayer(new[] { "mylo", "mylo" });
AddPlayer(new[] { "niels", "niels" });

game.Start();
game.StopLaundryTimer();
game.StopLaundryTurnTimerAndStartRound();

while (true)
{
    string? command = Console.ReadLine();
    if (command == null)
    {
        continue;
    }

    string[] inputParts = command.Split(' ');
    Enum.TryParse(typeof(Command), inputParts[0], out object? inputCommand);

    //TODO: remove in production
    if (command == "ShowCards")
    {
        ShowEveryonesCards();
        continue;
    }

    switch (inputCommand)
    {
        case Command.Start:
            Start();
            break;
        case Command.AddPlayer:
            AddPlayer(inputParts);
            break;
        case Command.DirtyLaundry:
            DirtyLaundry(inputParts);
            break;
        case Command.WhiteLaundry:
            WhiteLaundry(inputParts);
            break;
        case Command.StopLaundryTimer:
            StopLaundryTimer();
            break;
        case Command.TurnsLaundry:
            TurnsLaundry(inputParts);
            break;
        case Command.Check:
            Check(inputParts);
            break;
        case Command.Fold:
            Fold(inputParts);
            break;
        case Command.Knock:
            Knock(inputParts);
            break;
        case Command.PlayCard:
            PlayCard(inputParts);
            break;
        case Command.StopLaundryTurnTimerAndStartLaundryTimer:
            StopLaundryTurnTimerAndStartLaundryTimer();
            break;
        case Command.StopLaundryTurnTimerAndStartRound:
            StopLaundryTurnTimerAndStartRound();
            break;
        default:
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Incorrect! List of available commands:");
            Console.WriteLine("---------------------");
            foreach (Command cmd in Enum.GetValues(typeof(Command)))
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
        ShowEveryonesCards();
    }
}

void ShowEveryonesCards()
{
    Console.WriteLine("---------------------");
    Console.WriteLine("Player cards");
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

void AddPlayer(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Command.AddPlayer.ToString()} playername");
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
        Console.WriteLine($"Wrong command, please run: {Command.DirtyLaundry.ToString()} playerid");
        return;
    }

    StatusMessage statusMessage = game.DirtyLaundry(int.Parse(args[1]));
    if (!statusMessage.Success)
    {
        Console.WriteLine($"{statusMessage.Message}");
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
        Console.WriteLine($"Wrong command, please run: {Command.WhiteLaundry.ToString()} playerid");
        return;
    }

    StatusMessage statusMessage = game.WhiteLaundry(int.Parse(args[1]));
    if (!statusMessage.Success)
    {
        Console.WriteLine($"{statusMessage.Message}");
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
        Console.WriteLine($"Wrong command, please do: {Command.TurnsLaundry.ToString()} turnerId victimId");
        return;
    }

    StatusMessage statusMessage = game.TurnsLaundry(int.Parse(args[1]), int.Parse(args[2]));
    if (!statusMessage.Success)
    {
        Console.WriteLine($"{statusMessage.Message}");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} draait de was om van speler {args[2]}");
    Console.WriteLine();
    Console.WriteLine($"{args[2]} {statusMessage.Message}");
    Console.WriteLine("---------------------");
}

void StopLaundryTurnTimerAndStartLaundryTimer()
{
    if (!game.StopLaundryTurnTimerAndStartLaundryTimer())
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine("LaundryTurn timer stopped, laundry timer started again");
    Console.WriteLine("---------------------");
}

void StopLaundryTurnTimerAndStartRound()
{
    if (!game.StopLaundryTurnTimerAndStartRound())
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine("LaundryTurn timer stopped, Round has started");
    Console.WriteLine("---------------------");
}

void Check(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Command.Check.ToString()} playerid");
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
        Console.WriteLine($"Wrong command, please run: {Command.Fold.ToString()} playerid");
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
        Console.WriteLine($"Wrong command, please run: {Command.Knock.ToString()} playerid");
        return;
    }

    game.Knock(int.Parse(args[1]));
    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} klopt");
    Console.WriteLine("---------------------");
}

void PlayCard(string[] args)
{
    if (args.Length != 4)
    {
        Console.WriteLine($"Wrong command, please run: {Command.PlayCard.ToString()} playerid 7 c");
        return;
    }

    game.PlayCard(int.Parse(args[1]), args[2], args[3]);
    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} klopt");
    Console.WriteLine("---------------------");
}
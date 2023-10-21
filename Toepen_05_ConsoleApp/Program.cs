using Toepen_20_BusinessLogicLayer.Enums;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

Game game = new();

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

game.Start();
game.BlockLaundryCalls();
game.BlockLaundryTurnCallsAndStartRound();

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
    try
    {
        game.Start();
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine("GAME STARTED");
    Console.WriteLine("---------------------");
    ShowEveryonesCards();
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

    try
    {
        game.AddPlayer(new Player(args[1]));
    }
    catch (TooManyPlayersException e)
    {
        Console.WriteLine("Lobby is full");
    }
    catch (AlreadyStartedException e)
    {
        Console.WriteLine("Game has already been started!");
    }
}

void DirtyLaundry(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Command.DirtyLaundry.ToString()} playerid");
        return;
    }

    try
    {
        game.PlayerCallsDirtyLaundry(int.Parse(args[1]));
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine(e);
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

    try
    {
        game.PlayerCallsWhiteLaundry(int.Parse(args[1]));
    }
    catch (InvalidOperationException e)
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
    try
    {
        game.BlockLaundryCalls();
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("Cant perform this action now");
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

    try
    {
        game.PlayerTurnsLaundry(int.Parse(args[1]), int.Parse(args[2]));
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} draait de was om van speler {args[2]}");
    Console.WriteLine("---------------------");
}

void StopLaundryTurnTimerAndStartLaundryTimer()
{
    try
    {
        game.BlockLaundryTurnCallsAndWaitForLaundryCalls();
    }
    catch (InvalidOperationException e)
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
    try
    {
        game.BlockLaundryTurnCallsAndStartRound();
    }
    catch (InvalidOperationException e)
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

    try
    {
        game.Check(int.Parse(args[1]));
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

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

    try
    {
        game.Fold(int.Parse(args[1]));
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} folds");
    Console.WriteLine("---------------------");
}

void Knock(string[] args)
{
    if (args.Length != 2)
    {
        Console.WriteLine($"Wrong command, please run: {Command.Knock.ToString()} playerid");
        return;
    }

    try
    {
        game.Knock(int.Parse(args[1]));
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} knocks");
    Console.WriteLine("---------------------");
}

void PlayCard(string[] args)
{
    if (args.Length != 4)
    {
        Console.WriteLine($"Wrong command, please run: {Command.PlayCard.ToString()} playerid 7 c");
        return;
    }

    try
    {
        game.PlayerPlaysCard(int.Parse(args[1]), args[2], args[3]);
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("Cant perform this action now");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Player {args[1]} plays {args[2]} {args[3]}");
    Console.WriteLine("---------------------");
}
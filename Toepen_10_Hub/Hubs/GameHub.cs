using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Toepen_10_Hub.Enums;
using Toepen_10_Hub.Interfaces;
using Toepen_10_Hub.Services;
using Toepen_10_Hub.ViewModels;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Hubs;

public class GameHub : Hub<IGameClient>
{
    private readonly IGameService _gameService;

    public GameHub(IDictionary<string, UserConnection> connections, IGameService gameService)
    {
        _gameService = gameService;
        _gameService.SetUserConnections(connections);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            _gameService.GetUserConnections().Remove(Context.ConnectionId);
            if (_gameService.GetUserConnections().Count(c => c.Value.RoomCode == userConnection.RoomCode) <= 0)
            {
                _gameService.RemoveGame(userConnection.RoomCode);
            }

            Clients.Group(userConnection.RoomCode).ReceiveMessage(null, $"{userConnection.UserName} has left");
            SendConnectedUsers(userConnection.RoomCode);

            return base.OnDisconnectedAsync(exception);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            await Clients.Group(userConnection.RoomCode).ReceiveMessage(userConnection.UserName, $"{message}");
        }
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        bool roomExists = _gameService.GetUserConnections().Any(c => c.Value.RoomCode == userConnection.RoomCode);
        Game game = roomExists ? _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode) : new Game(userConnection.RoomCode);
        if (!roomExists)
        {
            _gameService.AddGame(game);
        }

        string message = roomExists ? $"{userConnection.UserName} has joined the room" : $"A new room has been created: {userConnection.RoomCode}";

        try
        {
            game.AddPlayer(new Player(Context.ConnectionId, userConnection.UserName));
        }
        catch (AlreadyStartedException)
        {
            await SendFlashMessage(FlashType.Error, "Game already started");
            return;
        }
        catch (TooManyPlayersException)
        {
            await SendFlashMessage(FlashType.Error, "Game is full");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.RoomCode);
        _gameService.GetUserConnections()[Context.ConnectionId] = userConnection;

        await Clients.Group(userConnection.RoomCode).ReceiveMessage(null, message);
        await SendCurrentUser(userConnection.RoomCode);
        await SendConnectedUsers(userConnection.RoomCode);
    }

    // TODO: Handle exceptions
    public Task SendConnectedUsers(string room)
    {
        Game game = _gameService.Games.First(g => g.RoomCode == room);
        GameViewModel gameViewModel = GameTransformer.GameToViewModel(game, Context.ConnectionId);
        List<PlayerViewModel> players = gameViewModel.Players;

        return Clients.Group(room).ReceiveUsersInRoom(JsonSerializer.Serialize(players));
    }
    
    // TODO: Handle exceptions
    private Task SendCurrentUser(string room)
    {
        Game game = _gameService.Games.First(g => g.RoomCode == room);
        Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
        PlayerViewModel playerViewModel = GameTransformer.PlayerToViewModel(player, Context.ConnectionId);
        
        return Clients.Client(Context.ConnectionId).ReceiveConnectedUser(JsonSerializer.Serialize(playerViewModel));
    }

    private async Task SendFlashMessage(FlashType type, string message)
    {
        await Clients.Caller.ReceiveFlashMessage(type.ToString(), message);
    }

    private async Task SendCurrentGameInfo()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);

            List<string> usersInRoom = _gameService.GetUserConnections().Where(c => c.Value.RoomCode == game.RoomCode).Select(c => c.Key).ToList();

            foreach (string connectionId in usersInRoom)
            {
                GameViewModel gameViewModel = GameTransformer.GameToViewModel(game, connectionId);

                await Clients.Client(connectionId).ReceiveGame(JsonSerializer.Serialize(gameViewModel));
            }
        }
    }

    public async Task StartGame()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            try
            {
                game.Start();
                await SendCurrentGameInfo();
                await Clients.Group(userConnection.RoomCode).ReceiveMessage(null, "Game started");
            }
            catch (AlreadyStartedException)
            {
                await SendFlashMessage(FlashType.Error, "Game already started");
            }
            catch (NotEnoughPlayersException)
            {
                await SendFlashMessage(FlashType.Warning, "Not enough players");
            }
        }
    }

    public async Task CallDirtyLaundry()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
            try
            {
                game.PlayerCallsDirtyLaundry(player?.Id ?? 0);

                await SendCurrentGameInfo();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }
            catch (AlreadyCalledLaundryException)
            {
                await SendFlashMessage(FlashType.Error, "Je hebt al een was aangegeven");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
        }
    }

    public async Task CallWhiteLaundry()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
            try
            {
                game.PlayerCallsWhiteLaundry(player?.Id ?? 0);

                await SendCurrentGameInfo();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }
            catch (AlreadyCalledLaundryException)
            {
                await SendFlashMessage(FlashType.Error, "Je hebt al een was aangegeven");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
        }
    }

    public async Task TurnLaundry(int victimId)
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
            Player? victim = game.FindPlayerById(victimId);
            try
            {
                game.PlayerTurnsLaundry(player?.Id ?? 0, victim?.Id ?? 0);

                await SendCurrentGameInfo();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }
            catch (AlreadyTurnedException)
            {
                await SendFlashMessage(FlashType.Warning, "De was is al omgedraaid");
            }
            catch (CantPerformToSelfException)
            {
                await SendFlashMessage(FlashType.Error, "Kan deze actie niet op jezelf uitvoeren");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
        }
    }

    public async Task Knock()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
            try
            {
                game.PlayerKnocks(player?.Id ?? 0);

                await SendCurrentGameInfo();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
        }
    }

    public async Task Check()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
            try
            {
                game.PlayerChecks(player?.Id ?? 0);

                await SendCurrentGameInfo();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
        }
    }

    public async Task Fold()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
            try
            {
                game.PlayerFolds(player?.Id ?? 0);

                await SendCurrentGameInfo();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
        }
    }

    public async Task PlayCard(CardViewModel cardViewModel)
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            Player? player = game.FindPlayerByConnectionId(Context.ConnectionId);
            try
            {
                game.PlayerPlaysCard(player?.Id ?? 0, GameTransformer.CardViewModelToCard(cardViewModel));

                await SendCurrentGameInfo();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }
            catch (NotPlayersTurnException)
            {
                await SendFlashMessage(FlashType.Error, "Niet jouw beurt");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
            catch (CardDoesNotMatchSuitsException)
            {
                await SendFlashMessage(FlashType.Warning, "Je moet kleur bekennen");
            }
            catch (CardNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Kaart niet gevonden");
            }
        }
    }

    // TODO: remove or handle exceptions
    public async Task SkipLaundry()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);

            try
            {
                game.BlockLaundryCalls();
                game.BlockLaundryTurnCalls();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }

            await SendFlashMessage(FlashType.Success, "Was overgeslagen");
            await SendCurrentGameInfo();
        }
        else
        {
            await SendFlashMessage(FlashType.Error, "Speler is niet verbonden");
        }
    }

    // TODO: remove or handle exceptions
    public async Task SkipLaundryCalls()
    {
        if (_gameService.GetUserConnections().TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);

            try
            {
                game.BlockLaundryCalls();
            }
            catch (InvalidStateException)
            {
                await SendFlashMessage(FlashType.Error, "Deze actie kan nu niet uitgevoerd worden");
            }

            await SendFlashMessage(FlashType.Success, "Was overgeslagen");
            await SendCurrentGameInfo();
        }
        else
        {
            await SendFlashMessage(FlashType.Error, "Speler is niet verbonden");
        }
    }
}
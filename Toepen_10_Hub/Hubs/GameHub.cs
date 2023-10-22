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
    private readonly IDictionary<string, UserConnection> _connections; // .NET 8 constructor injection breaks this for some reason

    private readonly IGameService _gameService;

    public GameHub(IDictionary<string, UserConnection> connections, IGameService gameService)
    {
        _connections = connections;
        _gameService = gameService;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            _connections.Remove(Context.ConnectionId);
            if (_connections.Count(c => c.Value.RoomCode == userConnection.RoomCode) <= 0)
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            await Clients.Group(userConnection.RoomCode).ReceiveMessage(userConnection.UserName, $"{user}: {message}");
        }
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        bool roomExists = _connections.Any(c => c.Value.RoomCode == userConnection.RoomCode);
        Game game = roomExists ? _gameService.Games.First(g => g.RoomId == userConnection.RoomCode) : new Game(userConnection.RoomCode);
        if (!roomExists)
        {
            _gameService.AddGame(game);
        }

        string message = roomExists ? $"{userConnection.UserName} has joined the room" : $"A new room has been created: {userConnection.RoomCode}";

        try
        {
            game.AddPlayer(new Player(Context.ConnectionId, userConnection.UserName));
        }
        catch (AlreadyStartedException e)
        {
            await SendFlashMessage(FlashType.Error, "Game already started");
            return;
        }
        catch (TooManyPlayersException e)
        {
            await SendFlashMessage(FlashType.Error, "Game is full");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.RoomCode);
        _connections[Context.ConnectionId] = userConnection;

        await Clients.Group(userConnection.RoomCode).ReceiveMessage(null, message);
        await SendConnectedUsers(userConnection.RoomCode);
    }

    public Task SendConnectedUsers(string room)
    {
        IEnumerable<string> users = _connections.Values
            .Where(c => c.RoomCode == room)
            .Select(c => c.UserName);

        return Clients.Group(room).ReceiveUsersInRoom(users);
    }

    private async Task SendFlashMessage(FlashType type, string message)
    {
        await Clients.Caller.ReceiveFlashMessage(type.ToString(), message);
    }

    private async Task SendCurrentGameInfo()
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
            GameTransformer gameTransformer = new();

            List<string> usersInRoom = _connections.Where(c => c.Value.RoomCode == userConnection.RoomCode).Select(c => c.Key).ToList();

            foreach (string connectionId in usersInRoom)
            {
                GameViewModel gameViewModel = GameTransformer.GameToViewModel(game, connectionId);
                SetUserOrder(gameViewModel);

                // TODO: Send Names values instead of integers

                await Clients.Client(connectionId).ReceiveGame(JsonSerializer.Serialize(gameViewModel));
            }
        }
    }

    private static void SetUserOrder(GameViewModel gameViewModel)
    {
        int playersIndex = gameViewModel.Players.FindIndex(user => user.IsYou);

        if (playersIndex != -1)
        {
            List<PlayerViewModel> firstPart = gameViewModel.Players.GetRange(playersIndex, gameViewModel.Players.Count - playersIndex);
            List<PlayerViewModel> secondPart = gameViewModel.Players.GetRange(0, playersIndex);

            gameViewModel.Players.Clear();

            gameViewModel.Players.AddRange(firstPart);
            gameViewModel.Players.AddRange(secondPart);
        }
    }

    public async Task StartGame()
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
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
            catch (CardNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Kaart niet gevonden");
            }
            catch (PlayerNotFoundException)
            {
                await SendFlashMessage(FlashType.Error, "Speler niet gevonden");
            }
        }
    }
}
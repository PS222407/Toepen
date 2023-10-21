using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Toepen_10_Hub.Interfaces;
using Toepen_10_Hub.Services;
using Toepen_10_Hub.ViewModels;
using Toepen_20_BusinessLogicLayer.Exceptions;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Hubs;

public class GameHub : Hub
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

            Clients.Group(userConnection.RoomCode).SendAsync("ReceiveMessage", null, $"{userConnection.UserName} has left");
            SendConnectedUsers(userConnection.RoomCode);
            
            return base.OnDisconnectedAsync(exception);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            await Clients.Group(userConnection.RoomCode).SendAsync("ReceiveMessage", userConnection.UserName, $"{user}: {message}");
        }
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        string message;
        bool roomExists = _connections.Any(c => c.Value.RoomCode == userConnection.RoomCode);
        if (roomExists)
        {
            Game game = _gameService.Games.First(g => g.RoomId == userConnection.RoomCode);
            bool isSuccess = game.TryAddPlayer(new Player(Context.ConnectionId, userConnection.UserName));
            // TODO: handle failure
            message = $"{userConnection.UserName} has joined the room";
        }
        else
        {
            Game game = new(userConnection.RoomCode);
            bool isSuccess = game.TryAddPlayer(new Player(Context.ConnectionId, userConnection.UserName));
            // TODO: handle failure
            _gameService.AddGame(game);
            message = $"A new room has been created: {userConnection.RoomCode}";
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.RoomCode);
        _connections[Context.ConnectionId] = userConnection;

        await Clients.Group(userConnection.RoomCode).SendAsync("ReceiveMessage", null, message);
        await SendConnectedUsers(userConnection.RoomCode);
    }
    
    public Task SendConnectedUsers(string room)
    {
        IEnumerable<string> users = _connections.Values
            .Where(c => c.RoomCode == room)
            .Select(c => c.UserName);

        return Clients.Group(room).SendAsync("UsersInRoom", users);
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
                await Clients.Group(userConnection.RoomCode).SendAsync("ReceiveMessage", null, "Game started");
            }
            catch (AlreadyStartedException e)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", null, "Warning! Game already started");
            }
            catch (NotEnoughPlayersException e)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", null, "Warning! Not enough players");
            }
        }
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
                GameViewModel gameViewModel = gameTransformer.GameToViewModel(game, connectionId);
                SetUserOrder(gameViewModel);
                
                // TODO: Send Names values instead of integers
                
                await Clients.Client(connectionId).SendAsync(
                    "ReceiveGame",
                    null,
                    null,
                    JsonSerializer.Serialize(gameViewModel)
                );
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
}
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Toepen_10_Hub.Interfaces;
using Toepen_10_Hub.Services;
using Toepen_10_Hub.ViewModels;

namespace Toepen_10_Hub.Hubs;

public class GameHub : Hub
{
    private readonly IDictionary<string, UserConnection> _connections; // .NET 8 constructor injection breaks this for some reason
    
    private readonly IGameService _gameService;

    private readonly List<CardViewModel> _cardViewModels1 = new()
    {
        new CardViewModel
        {
            Suit = "Spades",
            Value = 8
        },
        new CardViewModel
        {
            Suit = "Hearts",
            Value = 6
        },
    };

    private readonly List<CardViewModel> _cardViewModels2 = new()
    {
        new CardViewModel
        {
            Suit = "Clubs",
            Value = 3
        },
        new CardViewModel
        {
            Suit = "Diamonds",
            Value = 19
        },
    };

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
            Clients.Group(userConnection.RoomCode).SendAsync("ReceiveMessage", null, $"{userConnection.UserName} has left");

            return base.OnDisconnectedAsync(exception);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message) // Acts as player input action such as playing a card from hand
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            GameViewModel game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            List<string> usersInRoom = _connections.Where(c => c.Value.RoomCode == userConnection.RoomCode).Select(c => c.Key).ToList();

            foreach (string userId in usersInRoom)
            {
                GameViewModel gameCopy = JsonSerializer.Deserialize<GameViewModel>(JsonSerializer.Serialize(game))!;

                string playersHand = JsonSerializer.Serialize(gameCopy.Players.First(p => p.Id == userId).CardViewModels);
                for (int i = 0; i < gameCopy.Players.Count; i++)
                {
                    PlayerViewModel player = gameCopy.Players[i];
                    for (int j = 0; j < player.CardViewModels.Count; j++)
                    {
                        CardViewModel card = player.CardViewModels[j];
                        int index = player.CardViewModels.FindIndex(c => c == card);
                        player.CardViewModels[index] = new CardViewModel { Value = 0, Suit = "X" };
                    }
                }

                string gameWithoutHands = JsonSerializer.Serialize(gameCopy);

                await Clients.Client(userId).SendAsync(
                    "ReceiveMessage",
                    userConnection.UserName,
                    $"{user}: {message}",
                    gameWithoutHands,
                    playersHand
                );
            }
        }
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        string message;
        bool roomExists = _connections.Any(c => c.Value.RoomCode == userConnection.RoomCode);
        if (roomExists)
        {
            GameViewModel game = _gameService.Games.First(g => g.RoomCode == userConnection.RoomCode);
            game.Players.Add(new PlayerViewModel
            {
                Id = Context.ConnectionId,
                Name = userConnection.UserName,
                CardViewModels = _cardViewModels1
            });
            message = $"{userConnection.UserName} has joined the room";
        }
        else
        {
            GameViewModel game = new()
            {
                RoomCode = userConnection.RoomCode
            };
            game.Players.Add(new PlayerViewModel
            {
                Id = Context.ConnectionId,
                Name = userConnection.UserName,
                CardViewModels = _cardViewModels2
            });
            _gameService.Games.Add(game);
            message = $"A new room has been created: {userConnection.RoomCode}";
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.RoomCode);
        _connections[Context.ConnectionId] = userConnection;

        await Clients.Group(userConnection.RoomCode).SendAsync("ReceiveMessage", null, message);
    }
}
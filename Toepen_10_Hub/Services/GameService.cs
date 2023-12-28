using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Toepen_10_Hub.Hubs;
using Toepen_10_Hub.Interfaces;
using Toepen_10_Hub.ViewModels;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Services;

public class GameService : IGameService
{
    private IDictionary<string, UserConnection> _connections; // .NET 8 constructor injection breaks this for some reason

    private readonly IHubContext<GameHub, IGameClient> _hubContext;

    private Timer _timer;

    private readonly List<Game> _games = new();

    public IReadOnlyList<Game> Games => _games;

    public GameService(IHubContext<GameHub, IGameClient> hubContext)
    {
        _hubContext = hubContext;
        _timer = new Timer(GameTimerCallback, null, 0, 1000);
    }

    public IDictionary<string, UserConnection> GetUserConnections()
    {
        return _connections;
    }

    public void SetUserConnections(IDictionary<string, UserConnection> connections)
    {
        _connections = connections;
    }

    private void GameTimerCallback(object? state)
    {
        List<Game> copyOfGames;
        lock (_games)
        {
            copyOfGames = new List<Game>(_games);
        }
        
        foreach (Game game in copyOfGames)
        {
            TimerInfo? timerInfo = game.TimerCallback();
            if (timerInfo == null)
            {
                continue;
            }

            if (timerInfo.Seconds != -1)
            {
                _hubContext.Clients.Group(game.RoomCode).ReceiveCountdown(JsonSerializer.Serialize(timerInfo));
            }

            if (timerInfo.Done)
            {
                List<string> usersInRoom = GetUserConnections().Where(c => c.Value.RoomCode == game.RoomCode).Select(c => c.Key).ToList();

                foreach (string connectionId in usersInRoom)
                {
                    GameViewModel gameViewModel = GameTransformer.GameToViewModel(game, connectionId);

                    _hubContext.Clients.Client(connectionId).ReceiveGame(JsonSerializer.Serialize(gameViewModel));
                }
            }
        }
    }

    public void AddGame(Game game)
    {
        _games.Add(game);
    }

    public void RemoveGame(string roomCode)
    {
        _games.RemoveAll(game => game.RoomCode == roomCode);
    }
}
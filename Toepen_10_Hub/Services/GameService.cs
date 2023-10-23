using Microsoft.AspNetCore.SignalR;
using Toepen_10_Hub.Hubs;
using Toepen_10_Hub.Interfaces;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Services;

public class GameService : IGameService
{
    private readonly IHubContext<GameHub, IGameClient> _hubContext;
    
    private Timer _timer;
    
    private readonly List<Game> _games = new();

    public IReadOnlyList<Game> Games => _games;

    public GameService(IHubContext<GameHub, IGameClient> hubContext)
    {
        _hubContext = hubContext;
        _timer = new Timer(GameTimerCallback, null, 0, 1000);
    }

    private void GameTimerCallback(object? state)
    {
        foreach (Game game in _games)
        {
            _hubContext.Clients.Group(game.RoomCode).ReceiveCountdown(game.TimerCallback() ?? -1);
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
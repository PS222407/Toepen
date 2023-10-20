using Toepen_10_Hub.Interfaces;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Services;

public class GameService : IGameService
{
    private readonly List<Game> _games = new();

    public IReadOnlyList<Game> Games => _games;

    public void AddGame(Game game)
    {
        _games.Add(game);
    }

    public void RemoveGame(string roomCode)
    {
        _games.RemoveAll(game => game.RoomId == roomCode);
    }
}
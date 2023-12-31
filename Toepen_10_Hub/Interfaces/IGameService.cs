﻿using Toepen_10_Hub.Services;
using Toepen_20_BusinessLogicLayer.Models;

namespace Toepen_10_Hub.Interfaces;

public interface IGameService
{
    public IReadOnlyList<Game> Games { get; }

    public IDictionary<string, UserConnection> GetUserConnections();
    
    public void SetUserConnections(IDictionary<string, UserConnection> connections);
    
    public void AddGame(Game game);

    public void RemoveGame(string roomCode);
}
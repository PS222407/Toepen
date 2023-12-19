namespace Toepen_10_Hub.Interfaces;

public interface IGameClient
{
    Task ReceiveMessage(string? sender, string message);

    Task ReceiveGame(string game);
    
    Task ReceiveCountdown(string timerInfo);

    Task ReceiveUsersInRoom(string users);

    Task ReceiveTurnedCards(string cards);

    Task ReceiveConnectedUser(string user);

    Task ReceiveFlashMessage(string type, string message);
    
    Task ReceiveHasJoinedRoom(bool message);

    Task ReceiveGameLog(string gameLog);
}
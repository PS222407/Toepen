namespace Toepen_10_Hub.Interfaces;

public interface IGameClient
{
    Task ReceiveMessage(string? sender, string message);

    Task ReceiveGame(string game);
    
    Task ReceiveCountdown(int time);

    Task ReceiveUsersInRoom(string users);

    Task ReceiveFlashMessage(string type, string message);
}
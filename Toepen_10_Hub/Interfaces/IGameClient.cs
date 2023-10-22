namespace Toepen_10_Hub.Interfaces;

public interface IGameClient
{
    Task ReceiveMessage(string? sender, string message);
    
    Task ReceiveGame(string game);
    
    Task ReceiveUsersInRoom(IEnumerable<string> users);
    
    Task ReceiveFlashMessage(string type, string message);
}
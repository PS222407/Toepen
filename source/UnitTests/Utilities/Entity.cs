using System.Reflection;
using BusinessLogicLayer.Models;

namespace UnitTests.Utilities;

public static class Entity
{
    public static void SetIdOf(Player player, int idToSet)
    {
        Type playerType = player.GetType();
        FieldInfo idProperty = playerType.GetField($"<{nameof(Player.Id)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        idProperty.SetValue(player, idToSet);
    }

    public static void SetHandOf(Player player, List<Card> cards)
    {
        Type playerType = player.GetType();
        FieldInfo handProperty = playerType.GetField("_hand", BindingFlags.Instance | BindingFlags.NonPublic);
        handProperty.SetValue(player, cards);
    }
    
    public static void SetActivePlayerOf(Round round, Player player)
    {
        Type roundType = round.GetType();
        FieldInfo activePlayerProperty = roundType.GetField($"<{nameof(Round.ActivePlayer)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        activePlayerProperty.SetValue(round, player);
    }
    
    public static void SetStartedPlayerOf(Round round, Player player)
    {
        Type roundType = round.GetType();
        FieldInfo startedPlayerProperty = roundType.GetField($"<{nameof(Round.StartedPlayer)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        startedPlayerProperty.SetValue(round, player);
    }
}
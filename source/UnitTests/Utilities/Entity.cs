using System.Reflection;
using BusinessLogicLayer.Classes;

namespace UnitTests.Utilities;

public static class Entity
{
    public static void SetIdOf(Player player, int idToSet)
    {
        Type playerType = player.GetType();
        FieldInfo idProperty = playerType.GetField($"<{nameof(Player.Id)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        idProperty.SetValue(player, idToSet);
    }
}
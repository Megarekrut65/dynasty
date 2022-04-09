using System;
using Firebase.Database;

public static class ControllerFactory {

	public static Controller CreateController(string name, Player player,
			GameDependencies dependencies, Table table, Func<Card> takeCard) {
		return name switch {
			MEDIUM => new MoreCoinsBotController(player, dependencies, table, takeCard),
			ONLINE => new OnlineController(player, dependencies, table, takeCard,
				FirebaseDatabase.DefaultInstance.RootReference.Child(PrefabsKeys.ROOMS)
					.Child(PrefabsKeys.GetValue(PrefabsKeys.ROOM_NAME)).Child("players").Child(player.Key)),
			_ => new RandomBotController(player, dependencies, table, takeCard) //easy
		};
	}
	public const string ONLINE = "online";
	public const string MEDIUM = "medium";
}
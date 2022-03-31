using System; 

public static class ControllerFactory {

	public static Controller CreateController(string name, Player player,
			GameDependencies dependencies, Table table, Func<Card> takeCard) {
		return name switch {
			"medium" => new MoreCoinsBotController(player, dependencies, table, takeCard),
			_ => new RandomBotController(player, dependencies, table, takeCard)//easy
		};
	}
}
using System;
using UnityEngine;

public class ControllerFactory {

	public static Controller CreateController(string name, Player player,
			GameDependencies dependencies, Table table, Func<Card> takeCard) {
		switch (name) {
			case "easy":
				return new RandomBotController(player, dependencies, table, takeCard);
			case "medium":
				return new MoreCoinsBotController(player, dependencies, table, takeCard);
			default:
				return null;
		}
	}
}
using System;
using UnityEngine;

public class RandomBotController : BotController {
	public RandomBotController(Player player, GameDependencies dependencies,
		Table table, Func<Card> takeCard) : base(player, dependencies, table, takeCard) {
		Debug.Log("Random");
	}

	protected override SelectObjectData<Card> SelectCard() {
		var data = SelectManager.SelectData.selectingCards;
		return data.Count == 0 ? null : data[UnityEngine.Random.Range(0, data.Count)];
	}
}
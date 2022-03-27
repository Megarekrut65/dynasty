using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RandomBotController : BotController {
	public RandomBotController(Player player, GameDependencies dependencies,
		Table table, Func<Card> takeCard) : base(player, dependencies, table, takeCard) { }

	protected override SelectObjectData<Card> SelectCard() {
		var data = SelectManager.SelectData.selectingCards;
		if (data.Count == 0) return null;
		return data[UnityEngine.Random.Range(0, data.Count)];
	}
}
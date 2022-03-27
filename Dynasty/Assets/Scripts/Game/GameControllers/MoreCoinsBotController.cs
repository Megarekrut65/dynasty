using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoreCoinsBotController : BotController {
	public MoreCoinsBotController(Player player, GameDependencies dependencies,
		Table table, Func<Card> takeCard) : base(player, dependencies, table, takeCard) { }

	protected override SelectObjectData<Card> SelectCard() {
		return FindBestSelect();
	}

	private SelectObjectData<Card> FindBestSelect() {
		var type = SelectManager.SelectData.lastType;
		switch (type) {
			case "mix": return FindBestMix();
			case "move": return FindBestMove();
			case "cover": return FindBestCover();
			case "drop": return FindBestDrop();
			default: return null;
		}
	}
	private int CountAmount(SelectObjectData<Card> card) {
		int amount = card.obj.data.amount;
		if (card.owner.Equals(player)) amount = -2 * amount;
		return amount;
	}
	private SelectObjectData<Card> FindBestCover() {
		var data = SelectManager.SelectData.selectingCards;
		SelectObjectData<Card> best = data[0];
		int max = CountAmount(best);
		for (int i = 1; i < data.Count; i++) {
			int amount = CountAmount(data[i]);
			if (amount > max) {
				max = amount;
				best = data[i];
			}
		}
		return best;
	}
	private SelectObjectData<Card> FindBestMove() {
		return null;
	}
	private SelectObjectData<Card> FindBestMix() {
		return null;
	}
	private SelectObjectData<Card> FindBestDrop() {
		return null;
	}
}
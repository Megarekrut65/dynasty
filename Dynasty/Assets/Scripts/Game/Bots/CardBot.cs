using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class CardBot {
	private Player player;
	private GameDependencies dependencies;
	private Table table;
	private Func<Card> takeCard;

	public CardBot(Player player, GameDependencies dependencies, Table table, Func<Card> takeCard) {
		this.player = player;
		this.dependencies = dependencies;
		this.table = table;
		this.takeCard = takeCard;
		dependencies.roundManager.Next += Next;
	}
	public void Next() {
		Player next = dependencies.roundManager.WhoIsNextPlayer();
		if (next.nickname != this.player.nickname) return;
		dependencies.cameraMove.StartCoroutine(ClickOnCard());
	}
	IEnumerator ClickOnCard() {
		Card card = takeCard();
		yield return new WaitForSeconds(2f);
		if (card != null) {
			if (card.key == "inevitable-end") {
				Card avoid = table.FindCardInPlayer(player, "avoid-inevitable");
				if (avoid != null) yield return Click(avoid.obj.GetComponent<CardClick>());
				else yield return Click(card.obj.GetComponent<CardClick>());
			} else {
				yield return Click(card.obj.GetComponent<CardClick>());
				if (card.needSelect) {
					if (!SelectManager.SelectData.toOwner) {
						yield return new WaitForSeconds(1f);
						if (SelectManager.SelectData.selectingPlayers.Count > 0)
							yield return Click(SelectManager.SelectData.
								selectingPlayers[UnityEngine.Random.Range(0,
									SelectManager.SelectData.selectingPlayers.Count)].selectClick);
					}
					var data = RandomSelect();
					yield return new WaitForSeconds(1f);
					if (data != null) {
						yield return new WaitForSeconds(1f);
						yield return Click(data.selectClick);
					}
				}
			}
		}
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
	private SelectObjectData<Card> FindBestCover() {
		return null;
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
	private SelectObjectData<Card> RandomSelect() {
		var data = SelectManager.SelectData.selectingCards;
		if (data.Count == 0) return null;
		return data[UnityEngine.Random.Range(0, data.Count)];
	}
	IEnumerator Click(object cardClick) {
		(cardClick as IPointerDownHandler).OnPointerDown(null);
		yield return new WaitForSeconds(0.1f);
		(cardClick as IPointerUpHandler).OnPointerUp(null);
	}
}
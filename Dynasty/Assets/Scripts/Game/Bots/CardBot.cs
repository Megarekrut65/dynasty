using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class CardBot {
	private Player player;
	private GameManager gameManager;
	private Table table;
	private Func<Card> takeCard;

	public CardBot(Player player, GameManager gameManager, Table table, Func<Card> takeCard) {
		this.player = player;
		this.gameManager = gameManager;
		this.table = table;
		this.takeCard = takeCard;
		gameManager.Next += Next;
	}
	public void Next() {
		Player next = gameManager.GetNextPlayer();
		if (next.nickname != this.player.nickname) return;
		gameManager.StartCoroutine(ClickOnCard());
	}
	IEnumerator ClickOnCard() {
		Card card = takeCard();
		yield return new WaitForSeconds(gameManager.WaitTime);
		if (card != null) {
			if (card.key == "inevitable-end") {
				Card avoid = table.FindCardInPlayer(player, "avoid-inevitable");
				if (avoid != null) yield return Click(avoid.obj.GetComponent<CardClick>());
				else yield return Click(card.obj.GetComponent<CardClick>());
			} else {
				yield return Click(card.obj.GetComponent<CardClick>());
				if (card.needSelect) {
					card.needSelect = false;
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
	private SelectData FindBestSelect() {
		var type = SelectManager.LastType;
		switch (type) {
			case "mix": return FindBestMix();
			case "move": return FindBestMove();
			case "cover": return FindBestCover();
			case "drop": return FindBestDrop();
			default: return null;
		}
	}
	private SelectData FindBestCover() {
		return null;
	}
	private SelectData FindBestMove() {
		return null;
	}
	private SelectData FindBestMix() {
		return null;
	}
	private SelectData FindBestDrop() {
		return null;
	}
	private SelectData RandomSelect() {
		var data = SelectManager.LastSelecting;
		if (data.Count == 0) return null;
		return data[UnityEngine.Random.Range(0, data.Count)];
	}
	IEnumerator Click(object cardClick) {
		(cardClick as IPointerDownHandler).OnPointerDown(null);
		yield return new WaitForSeconds(0.1f);
		(cardClick as IPointerUpHandler).OnPointerUp(null);
	}
}
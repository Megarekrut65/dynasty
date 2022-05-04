using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class EntityController {
	protected Player player;
	public Player Player => player;
	protected GameDependencies dependencies;
	protected Table table;
	protected Func<Card> takeCard;

	public EntityController(Player player, GameDependencies dependencies, Table table, Func<Card> takeCard) {
		this.player = player;
		this.dependencies = dependencies;
		this.table = table;
		this.takeCard = takeCard;
		dependencies.roundManager.Next += Next;
	}

	protected virtual void Next() {
		Player next = dependencies.roundManager.WhoIsNextPlayer();
		if (!next.Equals(player)) return;
		dependencies.cameraMove.StartCoroutine(ClickOnCard());
	}
	protected abstract IEnumerator InevitableEnd(Card card);
	protected abstract SelectObjectData<GameObject> SelectPlayer();
	protected abstract SelectObjectData<Card> SelectCard();
	private IEnumerator ClickOnCard() {
		Card card = takeCard();
		yield return new WaitForSeconds(2f);
		if (card != null) {
			if (card.key == "inevitable-end") {
				yield return InevitableEnd(card);
			} else {
				yield return Click(card.obj.GetComponent<CardClick>());
				if (card.needSelect) {
					if (!SelectManager.SelectData.toOwner) {
						yield return new WaitForSeconds(1f);
						if (SelectManager.SelectData.selectingPlayers.Count > 0)
							yield return Click(SelectPlayer().selectClick);
					}
					var data = SelectCard();
					yield return new WaitForSeconds(1f);
					if (data != null) {
						yield return new WaitForSeconds(1f);
						yield return Click(data.selectClick);
					}
				}
			}
		}
	}
	protected IEnumerator Click(object cardClick) {
		(cardClick as IPointerDownHandler)?.OnPointerDown(null);
		yield return new WaitForSeconds(0.1f);
		(cardClick as IPointerUpHandler)?.OnPointerUp(null);
	}
}
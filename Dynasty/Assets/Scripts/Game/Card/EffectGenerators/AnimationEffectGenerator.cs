using System;
using UnityEngine;

public class AnimationEffectGenerator {
	protected GameManager gameManager;
	protected CardManager cardManager;
	protected Table table;
	protected CardAnimationManager animationManager;

	public AnimationEffectGenerator(GameManager gameManager,
		CardManager cardManager, Table table, CardAnimationManager animationManager) {
		this.gameManager = gameManager;
		this.cardManager = cardManager;
		this.table = table;
		this.animationManager = animationManager;
	}

	public void AddCardToPlayerAnimated(Card card, Player player, Action end) {
		animationManager.PlayCardHideShowAnimation(card.obj, () => {
			player.AddCard(card);
			table.AddCardToPlayer(player, card);
		}, end);
	}
	public void InsertCardToDeskAnimated(Card card, Action end) {
		animationManager.PlayCardToDesk(card.obj, () => {
			cardManager.DeleteCardFromTable(card);
			table.InsertToDesk(card);
			end();
		});
	}
	public void InsertPlayerCardToDeskAnimated(Card card, Action end) {
		animationManager.PlayCardHideAnimation(card.obj, () => {
			cardManager.DeleteCardFromTable(card);
			table.InsertToDesk(card);
			end();
		});
	}
	public void DropCardFromPlayerAnimated(Card card, Player player, Action end) {
		DropAnimated(card, () => {
			table.DropCardFromPlayer(player, card);
			end();
		});
	}
	public void DropCardAnimated(Card card, Action end) {
		DropAnimated(card, () => {
			table.DropCard(card);
			end();
		});
	}
	private void DropAnimated(Card card, Action action) {
		animationManager.PlayDropCard(card.obj, () => {
			cardManager.DeleteCardFromTable(card);
			action();
		});
	}
}
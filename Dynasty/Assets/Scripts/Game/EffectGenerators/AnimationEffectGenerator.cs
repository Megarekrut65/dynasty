using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class AnimationEffectGenerator {
	private CardController cardController;
	private Table table;
	private CardAnimationManager animationManager;

	public AnimationEffectGenerator(CardController cardController,
			Table table, CardAnimationManager animationManager) {
		this.cardController = cardController;
		this.table = table;
		this.animationManager = animationManager;
	}
	public void AddCardToPlayerAnimated(Card card, Player player, Action end) {
		animationManager.PlayCardHideShowAnimation(card?.obj, () => {
			player.AddCard(card);
			table.AddCardToPlayer(player, card);
		}, end);
	}
	public void InsertCardToDeskAnimated(Card card, Action end) {
		animationManager.PlayCardToDeskAnimation(card?.obj, () => {
			cardController.DeleteCardFromTable(card);
			table.InsertToDesk(card);
			end();
		});
	}
	public void InsertPlayerCardToDeskAnimated(Card card, Action end) {
		animationManager.PlayCardHideAnimation(card?.obj, () => {
			cardController.DeleteCardFromTable(card);
			table.InsertToDesk(card);
			end();
		});
	}
	public void CoverCardAnimated(Card under, Card top, Action end) {
		animationManager.PlayCoverCardAnimation(top?.obj, () => {
			table.CoverCard(under, top);
			cardController.CoverCard(under, top);
		}, end);
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
		animationManager.PlayDropCardAnimation(card?.obj, () => {
			cardController.DeleteCardFromTable(card);
			action();
		});
	}
	public void TakeAllAnimated(Player player, List<Card> cards, Action end) {
		animationManager.StartCoroutine(TakeAll(player, cards, end));
	}

	private IEnumerator TakeAll(Player player, List<Card> cards, Action end) {
		foreach (var card in cards) {
			yield return new WaitForSeconds(0.4f);
			AddCardToPlayerAnimated(card, player, GameAction.Empty);
		}
		end();
	}
	public void MixAllAnimated(List<Card> cards, Action end) {
		animationManager.StartCoroutine(MixAll(cards, end));
	}

	private IEnumerator MixAll(List<Card> cards, Action end) {
		foreach (var card in cards) {
			yield return new WaitForSeconds(0.4f);
			InsertPlayerCardToDeskAnimated(card, GameAction.Empty);
		}
		end();
	}
	public void PulsationCardAnimated(Card card) {
		animationManager.PlayPulsationCardAnimation(card?.obj, GameAction.Empty);
	}
	public void CountAmountAnimated(Player player, Card card) {
		animationManager.PlayCountAmountAnimation(card?.obj, () => {
			player.AddCoins(card.data.amount);
			DropCardAnimated(card, () => {
				for (var i = card; i != null; i = i?.underCard) {
					Object.Destroy(i.obj);
				}
			});
		});
	}
}
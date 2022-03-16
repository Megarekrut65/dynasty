using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimpleEffectsGenerator {
	protected AnimationEffectGenerator anim;
	protected GameManager gameManager;
	protected CardManager cardManager;
	protected Table table;

	public SimpleEffectsGenerator(GameManager gameManager,
		CardManager cardManager, Table table, AnimationEffectGenerator anim) {
		this.gameManager = gameManager;
		this.cardManager = cardManager;
		this.table = table;
		this.anim = anim;
	}
	protected Func<bool> CardMoreEffect(int more, Player player, Card card, bool call = true) {
		return () => {
			gameManager.AddCount(more);
			if (call) return CardEffect(player, card)();
			return true;
		};
	}
	protected Func<bool> DropEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			Card take = table.GetCardFromPlayer(key);
			if (take != null)
				anim.DropCardAnimated(take, CardEffectAction(call, player, card));
			else CardEffectAction(call, player, card)();
			return true;
		};
	}
	protected Func<bool> TakeAwayCardEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			var take = table.GetCardFromPlayer(key);
			if (take != null)
				anim.AddCardToPlayerAnimated(take, player, CardEffectAction(call, player, card));
			else CardEffectAction(call, player, card)();
			return true;
		};
	}
	protected Func<bool> MoveToCardEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			Player owner = player;
			Player with = table.GetPlayerWithCard(key);
			if (with != null) owner = with;
			if (call) return CardEffect(owner, card)();
			return true;
		};
	}
	protected Func<bool> MixEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			Card mix = table.GetCardFromPlayer(key);
			if (mix != null)
				anim.InsertPlayerCardToDeskAnimated(mix, CardEffectAction(call, player, card));
			else CardEffectAction(call, player, card)();
			return true;
		};
	}
	// protected Func<bool> CardEffect(Player player, Card card, bool callNext = true) {
	// 	if (card.type == CardType.MONSTER) {
	// 		return () => {
	// 			selectManager.SelectEnemy(player, gameManager.GetEnemies(player), (pl) => {
	// 				ClickEffect(pl, card, callNext)();
	// 			}, gameManager.IsPlayer(player));
	// 			return true;
	// 		};
	// 	}
	// 	return ClickEffect(player, card, callNext);
	// }
	protected Func<bool> CardEffect(Player player, Card card, bool callNext = true) {
		return () => {
			//Use effect
			//after effect
			if (card.data.type == "A") {
				player.AddCard(card);
				if (card.data.mix == "yes") anim.InsertCardToDeskAnimated(card, CallNext(callNext));
				else anim.DropCardFromPlayerAnimated(card, player, CallNext(callNext));
			} else anim.AddCardToPlayerAnimated(card, player, CallNext(callNext));
			return true;
		};
	}
	protected Action CardEffectAction(bool call, Player player, Card card) {
		return () => {
			if (call) CardEffect(player, card)();
		};
	}
	protected Action CallNext(bool callNext = true) {
		return () => {
			if (callNext) gameManager.StartCoroutine(Next());
		};
	}
	IEnumerator Next() {
		yield return new WaitForSeconds(1f);
		gameManager.CallNext();
	}

}
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
	protected Func<bool> CardMoreEffect(int more, Player player, Card card, bool other = true) {
		return () => {
			gameManager.AddCount(more);
			if (other) return OtherEffect(player, card)();
			return true;
		};
	}
	protected Func<bool> DropEffect(string key, Player player, Card card, bool other = true) {
		return () => {
			Card take = table.GetCardFromPlayer(key);
			if (take != null)
				anim.DropCardAnimated(take, Other(other, player, card));
			else Other(other, player, card)();
			return true;
		};
	}
	protected Func<bool> TakeAwayCardEffect(string key, Player player, Card card, bool other = true) {
		return () => {
			Card take = table.GetCardFromPlayer(key);
			if (take != null)
				anim.AddCardToPlayerAnimated(take, player, Other(other, player, card));
			else Other(other, player, card)();
			return true;
		};
	}
	protected Func<bool> MoveToCardEffect(string key, Player player, Card card, bool other = true) {
		return () => {
			Player owner = player;
			Player with = table.GetPlayerWithCard(key);
			if (with != null) owner = with;
			if (other) return OtherEffect(owner, card)();
			return true;
		};
	}
	protected Func<bool> MixEffect(string key, Player player, Card card, bool other = true) {
		return () => {
			Card mix = table.GetCardFromPlayer(key);
			if (mix != null)
				anim.InsertPlayerCardToDeskAnimated(mix, Other(other, player, card));
			else Other(other, player, card)();
			return true;
		};
	}
	protected Func<bool> OtherEffect(Player player, Card card, bool callNext = true) {
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
	protected Action Other(bool other, Player player, Card card) {
		return () => {
			if (other) OtherEffect(player, card)();
		};
	}
	protected Action CallNext(bool callNext) {
		return () => {
			if (callNext) gameManager.StartCoroutine(Next());
		};
	}
	IEnumerator Next() {
		yield return new WaitForSeconds(0.5f);
		gameManager.CallNext();
	}
	protected void TakeAllAnimated(Player player, List<Card> cards) {
		gameManager.StartCoroutine(TakeAll(player, cards));
	}
	IEnumerator TakeAll(Player player, List<Card> cards) {
		foreach (var card in cards) {
			yield return new WaitForSeconds(0.5f);
			anim.AddCardToPlayerAnimated(card, player, GameAction.EMPTY);
		}
		CallNext(true)();
	}
	protected void MixAllAnimated(List<Card> cards) {
		gameManager.StartCoroutine(MixAll(cards));
	}
	IEnumerator MixAll(List<Card> cards) {
		foreach (var card in cards) {
			yield return new WaitForSeconds(0.4f);
			anim.InsertPlayerCardToDeskAnimated(card, GameAction.EMPTY);
		}
		CallNext(true)();
	}
}
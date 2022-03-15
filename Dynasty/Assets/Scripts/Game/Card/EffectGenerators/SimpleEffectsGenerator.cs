using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimpleEffectsGenerator {
	protected AnimationEffectGenerator anim;
	protected GameManager gameManager;
	protected CardManager cardManager;
	protected Table table;
	protected SelectManager selectManager;

	public SimpleEffectsGenerator(GameManager gameManager,
		CardManager cardManager, Table table, AnimationEffectGenerator anim) {
		this.gameManager = gameManager;
		this.cardManager = cardManager;
		this.table = table;
		this.anim = anim;
		this.selectManager = new SelectManager(table);
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
	protected Func<bool> TakeAwayCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool other = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMove(predicate, player, players,
				(id) => {
					var take = table.GetCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, player, CallNext(other));
					else CallNext(other)();
				}, gameManager.IsPlayer(player));
			return OtherEffect(player, card, false)();
		};
	}
	protected Func<bool> MoveCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool other = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMoveToOther(predicate, player, players,
				(id, pl) => {
					var take = table.GetCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, pl, CallNext(other));
					else CallNext(other)();
				}, gameManager.IsPlayer(player));
			return OtherEffect(player, card, false)();
		};
	}
	protected Func<bool> TakeAwayCardEffect(string key, Player player, Card card, bool other = true) {
		return () => {
			var take = table.GetCardFromPlayer(key);
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
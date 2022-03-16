using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SelectEffectGenerator : SimpleEffectsGenerator {
	protected SelectManager selectManager;
	public SelectEffectGenerator(GameManager gameManager,
	CardManager cardManager, Table table, AnimationEffectGenerator anim)
		: base(gameManager, cardManager, table, anim) {
		this.selectManager = new SelectManager(table);
	}
	protected Func<bool> DropCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
								Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectDrop(predicate, player, players,
				(id) => {
					var drop = table.GetCardFromPlayer(id);
					if (drop != null)
						anim.DropCardAnimated(drop, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected Func<bool> MixCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
								Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMix(predicate, player, players,
				(id) => {
					var mix = table.GetCardFromPlayer(id);
					if (mix != null)
						anim.InsertPlayerCardToDeskAnimated(mix, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected Func<bool> TakeAwayCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMove(predicate, player, players,
				(id) => {
					var take = table.GetCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, player, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected Func<bool> MoveCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMoveToOther(predicate, player, players,
				(id, pl) => {
					var take = table.GetCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, pl, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
}
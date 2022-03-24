using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using CardEffect = System.Func<bool>;

public abstract class SimpleEffectsGenerator {
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
	public virtual CardEffect GetEffect(Player player, Card card) {
		switch (card.key) {
			case "light-wizard":
				return MixEffect("dark-wizard", player, card);
			case "dark-wizard":
				return MixEffect("light-wizard", player, card);
			case "dragon":
				return MoveToCardEffect("gold-mountain", player, card);
			case "hero":
				return DropEffect("dragon", player, card);
			case "castle":
			case "fairy-army":
			case "king-sword":
				return TakeAwayCardEffect("king", player, card);
			case "step-ahead":
				return CardMoreEffect(1, player, card);
			case "victim":
				return CardMoreEffect(2, player, card);
			default: {
					UnityEngine.Debug.Log(card.key);
					return CardEffect(player, card);
				}
		}
	}

	protected IEnumerator CountCoins(KeyValuePair<Player, List<Card>> item) {
		var player = item.Key;
		var cards = item.Value;
		foreach (var card in cards) {
			anim.CountAmountAnimated(player, card);
			yield return new WaitForSeconds(0.5f);
		}
		cards.Clear();
	}
	protected List<Player> PlayersWithoutCardFilter(
			List<Player> players, string cardName) {
		List<Player> res = new List<Player>();
		foreach (var player in players) {
			var card = table.FindCardInPlayer(player, cardName);
			if (card == null) {
				res.Add(player);
			} else {
				anim.PulsationCardAnimated(card);
			}
		}
		return res;
	}
	protected void BonusCoins(string mastBe, int coins, string owner) {
		Player player = table.FindPlayerWithCard(mastBe);
		if (player != null && player.nickname == owner) {
			var card = table.FindCardInPlayer(player, mastBe);
			anim.PulsationCardAnimated(card);
			player.AddCoins(coins);
		}
	}
	protected CardEffect CardMoreEffect(int more, Player player, Card card, bool call = true) {
		return () => {
			gameManager.AddCount(more);
			if (call) return CardEffect(player, card)();
			return true;
		};
	}
	protected CardEffect DropEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			Card take = table.RemoveCardFromPlayer(key);
			if (take != null) {
				if (take.underCard == null)
					anim.DropCardAnimated(take, CardEffectAction(call, player, card));
			} else CardEffectAction(call, player, card)();
			return true;
		};
	}
	protected CardEffect TakeAwayCardEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			var take = table.RemoveCardFromPlayer(key);
			if (take != null)
				anim.AddCardToPlayerAnimated(take, player, CardEffectAction(call, player, card));
			else CardEffectAction(call, player, card)();
			return true;
		};
	}
	protected CardEffect MoveToCardEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			Player owner = player;
			Player with = table.FindPlayerWithCard(key);
			if (with != null) owner = with;
			if (call) return CardEffect(owner, card)();
			return true;
		};
	}
	protected CardEffect MixEffect(string key, Player player, Card card, bool call = true) {
		return () => {
			Card mix = table.RemoveCardFromPlayer(key);
			if (mix != null)
				anim.InsertPlayerCardToDeskAnimated(mix, CardEffectAction(call, player, card));
			else CardEffectAction(call, player, card)();
			return true;
		};
	}
	protected CardEffect CardEffect(Player player, Card card, bool callNext = true) {
		return () => {
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
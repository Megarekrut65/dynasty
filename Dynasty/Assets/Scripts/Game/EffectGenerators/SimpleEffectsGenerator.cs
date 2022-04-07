using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using CardEffect = System.Func<bool>;

public abstract class SimpleEffectsGenerator {
	protected AnimationEffectGenerator anim;
	protected GameDependencies dependencies;
	protected CardController cardController;
	protected Table table;
	protected EffectLogger logger;
	protected MonoBehaviour behaviour;

	public SimpleEffectsGenerator(GameDependencies dependencies,
			CardController cardController, Table table, AnimationEffectGenerator anim) {
		this.dependencies = dependencies;
		this.cardController = cardController;
		this.table = table;
		this.anim = anim;
		this.logger = new EffectLogger(dependencies.logger, table);
		this.behaviour = dependencies.cameraMove;
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
					return CardEffect(player, card);
				}
		}
	}
	protected IEnumerator CountCoins(KeyValuePair<Player, List<Card>> item) {
		var player = item.Key;
		var cards = item.Value;
		int amount = 0;
		foreach (var card in cards) {
			amount += card.data.amount;
			anim.CountAmountAnimated(player, card);
			yield return new WaitForSeconds(0.5f);
		}
		logger.LogCoins(player, amount);
		yield return new WaitForSeconds(0.5f);
		logger.LogTotal(player);
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
		if (player != null && player.Nickname == owner) {
			var card = table.FindCardInPlayer(player, mastBe);
			anim.PulsationCardAnimated(card);
			player.AddCoins(coins);
			logger.LogCoins(player, coins);
		}
	}
	protected CardEffect CardMoreEffect(int more, Player player, Card card, bool callNext = true) {
		return () => {
			dependencies.roundManager.AddMoreRounds(more);
			return !callNext || CardEffect(player, card)();
		};
	}
	protected CardEffect DropEffect(string key, Player player, Card card, bool callNext = true) {
		return () => {
			logger.LogAction(player, key, "dropped");
			Card take = table.RemoveCardFromPlayer(key);
			if (take != null) {
				anim.DropCardAnimated(take, CardEffectAction(callNext, player, card));
			} else CardEffectAction(callNext, player, card)();
			return true;
		};
	}
	protected CardEffect TakeAwayCardEffect(string key, Player player, Card card, bool callNext = true) {
		return () => {
			logger.LogAction(player, key, "took-away");
			var take = table.RemoveCardFromPlayer(key);
			if (take != null) {
				anim.AddCardToPlayerAnimated(take, player, CardEffectAction(callNext, player, card));
			} else CardEffectAction(callNext, player, card)();
			return true;
		};
	}
	protected CardEffect MoveToCardEffect(string key, Player player, Card card, bool callNext = true) {
		return () => {
			Player owner = player;
			Player with = table.FindPlayerWithCard(key);
			if (with != null) {
				owner = with;
				Card c = table.FindCardInPlayer(with, key);
				anim.PulsationCardAnimated(c);
			}
			return !callNext || CardEffect(owner, card)();
		};
	}
	protected CardEffect MixEffect(string key, Player player, Card card, bool callNext = true) {
		return () => {
			logger.LogAction(player, key, "mixed");
			Card mix = table.RemoveCardFromPlayer(key);
			if (mix != null) {
				anim.InsertPlayerCardToDeskAnimated(mix, CardEffectAction(callNext, player, card));
			} else CardEffectAction(callNext, player, card)();
			return true;
		};
	}
	protected CardEffect CardEffect(Player player, Card card, bool callNext = true) {
		return () => {
			logger.LogGot(player, card);
			if (card.data.type == "A") {
				player.AddCard(card);
				if (card.data.mix == "yes") anim.InsertCardToDeskAnimated(card, CallNext(callNext));
				else anim.DropCardFromPlayerAnimated(card, player, CallNext(callNext));
			} else anim.AddCardToPlayerAnimated(card, player, CallNext(callNext));
			return true;
		};
	}
	protected Action CardEffectAction(bool callNext, Player player, Card card) {
		return () => {
			if (callNext) CardEffect(player, card)();
		};
	}
	protected Action CallNext(bool callNext = true) {
		return () => {
			if (callNext) behaviour.StartCoroutine(Next());
		};
	}

	private IEnumerator Next() {
		yield return new WaitForSeconds(1f);
		dependencies.roundManager.CallNextPlayer();
	}

}
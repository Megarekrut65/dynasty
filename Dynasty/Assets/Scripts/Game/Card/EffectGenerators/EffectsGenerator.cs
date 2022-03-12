using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EffectsGenerator : SimpleEffectsGenerator {
	public EffectsGenerator(GameManager gameManager,
	CardManager cardManager, Table table, AnimationEffectGenerator anim)
		: base(gameManager, cardManager, table, anim) { }
	public Func<bool> GetEffect(Player player, Card card) {
		switch (card.key) {
			case "inevitable-end":
				return InevitableEndEffect(card);
			case "avoid-inevitable":
				return AvoidInevitableEffect(player, card);
			case "light-wizard":
				return MixEffect("dark-wizard", player, card);
			case "dark-wizard":
				return MixEffect("light-wizard", player, card);
			case "gold-mountain":
				return GoldMountainEffect(player, card);
			case "dragon":
				return MoveToCardEffect("gold-mountain", player, card);
			case "hero":
				return DropEffect("dragon", player, card);
			case "dungeon-keys":
				return DungeonKeysEffect(player, card);
			case "secret-treasure":
				return SecretTreasureEffect(player, card);
			case "castle":
			case "fairy-army":
			case "king-sword":
				return TakeAwayCardEffect("king", player, card);
			case "swordsman":
			case "crusader":
			case "equestrian":
			case "archer":
				return KnightEffect(player, card);
			case "step-ahead":
				return CardMoreEffect(1, player, card);
			case "victim":
				return CardMoreEffect(2, player, card);
			case "snake":
				return SnakeEffect(player, card);
			case "cerberus":
				return CerberusEffect(player, card);
			case "four-elements":
				return FourElementsEffect(player, card);
			case "royal-poison":
				return RoyalPoisonEffect(player, card);
			case "dungeon":
				return DungeonEffect(player, card);
			default:
				return OtherEffect(player, card);
		}
	}
	private Func<bool> DungeonEffect(Player player, Card card) {
		return () => {
			selectManager.SelectAllCover(player, gameManager.Players,
				(id) => {
					UnityEngine.Debug.Log("id: " + id);
					CallNext(true)();
				}, gameManager.IsPlayer(player));
			return OtherEffect(player, card, false)();
		};
	}
	private Func<bool> RoyalPoisonEffect(Player player, Card card) {
		return () => {
			var knights = table.GetAllCardsFromPlayers(null, c => c.type == CardType.KNIGHT);
			var king = table.GetCardFromPlayer("king");
			knights.Add(king);
			anim.MixAllAnimated(knights, CallNext());
			return OtherEffect(player, card, false)();
		};
	}
	private Func<bool> FourElementsEffect(Player player, Card card) {
		return () => {
			var elements = table.GetAllCardsFromPlayers(player, c => c.type == CardType.WALL);
			anim.TakeAllAnimated(player, elements, CallNext());
			return OtherEffect(player, card, false)();
		};
	}
	private Func<bool> CerberusEffect(Player player, Card card) {
		return () => {
			var cerberus = table.FindAllCardsInPlayer(player, c => c.key == "cerberus");
			if (cerberus.Count >= 2)
				player.AddCoins(-2);
			return OtherEffect(player, card)();
		};
	}
	private Func<bool> SnakeEffect(Player player, Card card) {
		return () => {
			Player snake = table.GetPlayerWithCard("snake");
			if (snake != null && snake.nickname != player.nickname) {
				var snakes = table.GetAllCardsFromPlayer(snake, c => c.key == "snake");
				anim.TakeAllAnimated(player, snakes, CallNext());
				return OtherEffect(player, card, false)();
			}
			return OtherEffect(player, card)();
		};
	}
	private Func<bool> KnightEffect(Player player, Card card) {
		return () => {
			Player owner = player;
			Player king = table.GetPlayerWithCard("king");
			if (king != null) {
				owner = king;
			}
			return OtherEffect(owner, card)();
		};
	}
	private Func<bool> SecretTreasureEffect(Player player, Card card) {
		return () => {
			Player dungeon = table.GetPlayerWithCard("dungeon");
			if (dungeon != null && dungeon.nickname == player.nickname) {
				player.AddCoins(6);
			}
			return OtherEffect(player, card)();
		};
	}
	private Func<bool> DungeonKeysEffect(Player player, Card card) {
		return () => {
			return MixEffect("secret-treasure", player, card, false)() &&
				TakeAwayCardEffect("dungeon", player, card)();
		};
	}
	private Func<bool> GoldMountainEffect(Player player, Card card) {
		return () => {
			return DropEffect("gold-mountain", player, card, false)() &&
				TakeAwayCardEffect("dragon", player, card)();
		};
	}
	IEnumerator CountCoins(KeyValuePair<Player, List<Card>> item) {
		var player = item.Key;
		var cards = item.Value;
		foreach (var card in cards) {
			int amount = card.data.amount;
			player.AddCoins(amount);
			anim.DropCardAnimated(card, () => { MonoBehaviour.Destroy(card.obj); });
			yield return new WaitForSeconds(0.5f);
		}
		cards.Clear();
	}
	private Func<bool> InevitableEndEffect(Card card) {
		return () => {
			table.CountRCardCoins(item => { gameManager.StartCoroutine(CountCoins(item)); return true; });
			return false;
		};
	}
	private Func<bool> AvoidInevitableEffect(Player player, Card card) {
		return () => {
			Card currentCard = table.Current;
			if (currentCard != null && currentCard.key == "inevitable-end") {
				table.Current = null;
				anim.DropCardFromPlayerAnimated(card, player, () => {
					anim.InsertCardToDeskAnimated(currentCard, () => {
						gameManager.GameOver = false;
						CallNext(true)();
					});
				});
				return true;
			}
			return false;
		};
	}
}
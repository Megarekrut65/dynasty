using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class EffectsGenerator : SelectEffectGenerator {
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
			case "encampment":
				return EncampmentEffect(player, card);
			case "tower":
				return TowerEffect(player, card);
			case "robin-hood":
			case "reliable-plan":
				return TakeFromEnemySelectEffect(player, card);
			case "wolf-spirit":
			case "magic-sphere":
				return MoveToOtherSelectEffect(player, card);
			case "dracula":
				return DraculaEffect(player, card);
			case "green-pandora's-box":
			case "illusion":
			case "locusts":
				return MixEnemyCardSelectEffect(player, card);
			case "oversight":
			case "red-pandora's-box":
				return MixOwnCardSelectEffect(player, card);
			case "bear":
				return BearEffect(player, card);
			case "cemetery":
				return MixPlayersCardSelectEffect(player, card);
			case "hunter":
				return HunterEffect(player, card);
			case "artillery":
				return ArtilleryEffect(player, card);
			case "current":
			case "poison":
				return DropOwnCardSelectEffect(player, card);
			case "grail":
			case "hell":
				return DropEnemyCardSelectEffect(player, card);
			case "boarding-party":
				return BoardingPartyEffect(player, card);
			case "wand":
				return DropPlayersCardSelectEffect(player, card);
			case "killer":
				return KillerEffect(player, card);
			case "look-back":
				return TakeCardFromDropSelectEffect(player, card);
			case "dungeon":
				return DungeonEffect(player, card);
			case "bunker":
			case "curses":
				return CoverOwnCardSelectEffect(player, card);
			case "prison":
			case "magic-hat":
				return CoverPlayersCardSelectEffect(player, card);
			case "slime":
			case "black-cat":
				return CoverEnemyCardSelectEffect(player, card);
			case "hut":
				return HutEffect(player, card);
			case "explosion":
				return ExplosionEffect(player, card);
			default:
				return CardEffect(player, card);
		}
	}
	private Func<bool> ExplosionEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetFilter(
					CardFunctions.DROP, (c) => c.underCard != null),
				player, TotemFilter(gameManager.Players), card);
	}
	private Func<bool> HutEffect(Player player, Card card) {
		return CoverCardSelectEffect((c) => c.type == CardType.MONSTER,
							player, new List<Player>() { player }, card);
	}
	private Func<bool> DungeonEffect(Player player, Card card) {
		return () => {
			BonusCoins("secret-treasure", 6, player.nickname);
			return CoverPlayersCardSelectEffect(player, card)();
		};
	}
	private Func<bool> KillerEffect(Player player, Card card) {
		return DropCardSelectEffect((c) => c.type == CardType.KNIGHT,
					player, new List<Player>() { player }, card);
	}
	private Func<bool> BoardingPartyEffect(Player player, Card card) {
		var siren = table.FindCardInPlayer(player, "siren");
		if (siren == null) {
			return DropOwnCardSelectEffect(player, card);
		}
		anim.PulsationCardAnimated(siren);
		return CardEffect(player, card);
	}
	private Func<bool> ArtilleryEffect(Player player, Card card) {
		return () => {
			var castle = table.FindCardInPlayer(player, "black-sun-castle");
			if (castle == null) {
				return DropCardSelectEffect((c) => c.type == CardType.BUILDING || c.type == CardType.WALL,
						player, new List<Player>() { player }, card)();
			}
			anim.PulsationCardAnimated(castle);
			return CardEffect(player, card)();
		};
	}
	private Func<bool> HunterEffect(Player player, Card card) {
		return () => {
			var dracula = table.FindCardInPlayer(player, "dracula");
			if (dracula == null) {
				return DropCardSelectEffect((c) =>
					c.type == CardType.MONSTER, player,
					new List<Player>() { player }, card)();
			}
			anim.PulsationCardAnimated(dracula);
			return CardEffect(player, card)();
		};
	}
	private Func<bool> BearEffect(Player player, Card card) {
		return MixCardSelectEffect((c) => c.type == CardType.MONSTER,
			player, new List<Player>() { player }, card);
	}
	private Func<bool> DraculaEffect(Player player, Card card) {
		return MoveCardSelectEffect((c) => "batcrow".Contains(c.key), player, gameManager.Players, card);
	}
	private Func<bool> TowerEffect(Player player, Card card) {
		return TakeAwayCardSelectEffect((c) => c.key == "archer",
			player, gameManager.GetEnemies(player), card);
	}
	private Func<bool> EncampmentEffect(Player player, Card card) {
		return TakeAwayCardSelectEffect((c) => c.type == CardType.KNIGHT,
			player, gameManager.GetEnemies(player), card);
	}
	private Func<bool> RoyalPoisonEffect(Player player, Card card) {
		return () => {
			var knights = table.RemoveAllCardsFromPlayers(null, c => c.type == CardType.KNIGHT);
			var king = table.RemoveCardFromPlayer("king");
			knights.Add(king);
			anim.MixAllAnimated(knights, CallNext());
			return CardEffect(player, card, false)();
		};
	}
	private Func<bool> FourElementsEffect(Player player, Card card) {
		return () => {
			var elements = table.RemoveAllCardsFromPlayers(player, c => c.type == CardType.WALL);
			anim.TakeAllAnimated(player, elements, CallNext());
			return CardEffect(player, card, false)();
		};
	}
	private Func<bool> CerberusEffect(Player player, Card card) {
		return () => {
			var cerberus = table.FindAllCardsInPlayer(player, c => c.key == "cerberus");
			if (cerberus.Count >= 2) {
				foreach (var cer in cerberus) {
					anim.PulsationCardAnimated(cer);
				}
				player.AddCoins(-2);
			}
			return CardEffect(player, card)();
		};
	}
	private Func<bool> SnakeEffect(Player player, Card card) {
		return () => {
			Player snake = table.FindPlayerWithCard("snake");
			if (snake != null && snake.nickname != player.nickname) {
				var snakes = table.RemoveAllCardsFromPlayer(snake, c => c.key == "snake");
				anim.TakeAllAnimated(player, snakes, CallNext());
				return CardEffect(player, card, false)();
			}
			return CardEffect(player, card)();
		};
	}
	private Func<bool> KnightEffect(Player player, Card card) {
		return () => {
			Player owner = player;
			Player king = table.FindPlayerWithCard("king");
			if (king != null) {
				owner = king;
				var k = table.FindCardInPlayer(king, "king");
				anim.PulsationCardAnimated(k);
			}
			return CardEffect(owner, card)();
		};
	}
	private Func<bool> SecretTreasureEffect(Player player, Card card) {
		return () => {
			BonusCoins("dungeon", 6, player.nickname);
			return CardEffect(player, card)();
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
	private Func<bool> InevitableEndEffect(Card card) {
		return () => {
			table.CountRCardCoins(item => { gameManager.StartCoroutine(CountCoins(item)); });
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
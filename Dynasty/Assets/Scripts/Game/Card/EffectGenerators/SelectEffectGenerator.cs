using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using CardEffect = System.Func<bool>;

public abstract class SelectEffectGenerator : SimpleEffectsGenerator {
	protected SelectManager selectManager;

	public SelectEffectGenerator(GameManager gameManager,
	CardManager cardManager, Table table, AnimationEffectGenerator anim)
		: base(gameManager, cardManager, table, anim) {
		this.selectManager = new SelectManager(table);
	}
	public override CardEffect GetEffect(Player player, Card card) {
		switch (card.key) {
			case "robin-hood":
			case "reliable-plan":
				return TakeFromEnemySelectEffect(player, card);
			case "wolf-spirit":
			case "magic-sphere":
				return MoveToOtherSelectEffect(player, card);
			case "green-pandora's-box":
			case "illusion":
			case "locusts":
				return MixEnemyCardSelectEffect(player, card);
			case "oversight":
			case "red-pandora's-box":
				return MixOwnCardSelectEffect(player, card);
			case "cemetery":
				return MixPlayersCardSelectEffect(player, card);
			case "current":
			case "poison":
				return DropOwnCardSelectEffect(player, card);
			case "grail":
			case "hell":
				return DropEnemyCardSelectEffect(player, card);
			case "wand":
				return DropPlayersCardSelectEffect(player, card);
			case "look-back":
				return TakeCardFromDropSelectEffect(player, card);
			case "bunker":
			case "curses":
				return CoverOwnCardSelectEffect(player, card);
			case "prison":
			case "magic-hat":
				return CoverPlayersCardSelectEffect(player, card);
			case "slime":
			case "black-cat":
				return CoverEnemyCardSelectEffect(player, card);
			default:
				return base.GetEffect(player, card);
		}
	}
	protected CardEffect TakeCardFromDropSelectEffect(Player player, Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			gameManager.CameraMoveActive(false);
			gameManager.StartCoroutine(TakeFromDrop(player, card, call));
			return CardEffect(player, card, false)();
		};
	}
	private IEnumerator TakeFromDrop(Player player, Card card, bool call) {
		var cards = table.GetRCardsInDrop();
		foreach (var c in cards) {
			cardManager.CreateCard(c);
			gameManager.Scroll.AddToScroll(c.obj);
		}
		selectManager.SelectCard(player, cards, (id) => {
			gameManager.StartCoroutine(TakeAction(id, player, call));
		}, gameManager.IsPlayer(player));
		yield return null;
	}
	private IEnumerator TakeAction(int id, Player player, bool call) {
		var take = table.RemoveCardFromDrop(id);
		var cards = table.GetRCardsInDrop();
		foreach (var c in cards) {
			cardManager.DeleteCardFromTable(c);
		}
		Action after = () => {
			CallNext(call)();
			gameManager.CameraMoveActive(true);
		};
		if (take != null) {
			//gameManager.Scroll.ScrollTo(take.obj.GetComponent<RectTransform>());
			anim.AddCardToPlayerAnimated(take, player, after);
		} else after();
		yield return null;
	}
	protected CardEffect DropCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
								Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectDrop(predicate, player, players,
				(id) => {
					var drop = table.RemoveCardFromPlayer(id);
					if (drop != null)
						anim.DropCardAnimated(drop, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect CoverCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
							Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectCover(predicate, player, players,
				(id) => {
					var cover = table.FindCardInPlayers(id);
					if (cover != null)
						anim.CoverCardAnimated(cover, card, CallNext(call));
					else CardEffect(player, card)();
				}, gameManager.IsPlayer(player));
			return true;
		};
	}
	protected CardEffect MixCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
								Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMix(predicate, player, players,
				(id) => {
					var mix = table.RemoveCardFromPlayer(id);
					if (mix != null)
						anim.InsertPlayerCardToDeskAnimated(mix, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect TakeAwayCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMove(predicate, player, players,
				(id) => {
					var take = table.RemoveCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, player, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect MoveCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool call = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMoveToOther(predicate, player, players,
				(id, pl) => {
					var take = table.RemoveCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, pl, CallNext(call));
					else CallNext(call)();
				}, gameManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect CoverEnemyCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
							player, gameManager.GetEnemies(player), card);
	}
	protected CardEffect CoverOwnCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
							player, new List<Player>() { player }, card);
	}
	protected CardEffect CoverPlayersCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
				player, gameManager.Players, card);
	}
	protected List<Player> TotemFilter(List<Player> players) {
		return PlayersWithoutCardFilter(players, "totem");
	}
	protected CardEffect DropPlayersCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(gameManager.Players), card);
	}
	protected CardEffect DropEnemyCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(gameManager.GetEnemies(player)), card);
	}
	protected CardEffect DropOwnCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(new List<Player>() { player }), card);
	}
	protected CardEffect MixPlayersCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, gameManager.Players, card);
	}
	protected CardEffect MixOwnCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, new List<Player>() { player }, card);
	}
	protected CardEffect MixEnemyCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, gameManager.GetEnemies(player), card);
	}
	protected CardEffect MoveToOtherSelectEffect(Player player, Card card) {
		return MoveCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
							player, gameManager.Players, card);
	}
	protected CardEffect TakeFromEnemySelectEffect(Player player, Card card) {
		return TakeAwayCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
			player, gameManager.GetEnemies(player), card);
	}
}
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class SelectEffectGenerator : SimpleEffectsGenerator {
	protected SelectManager selectManager;
	public SelectEffectGenerator(GameManager gameManager,
	CardManager cardManager, Table table, AnimationEffectGenerator anim)
		: base(gameManager, cardManager, table, anim) {
		this.selectManager = new SelectManager(table);
	}
	protected Func<bool> TakeCardFromDropSelectEffect(Player player, Card card, bool call = true) {
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
	protected Func<bool> DropCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
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
	protected Func<bool> CoverCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
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
	protected Func<bool> MixCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
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
	protected Func<bool> TakeAwayCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
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
	protected Func<bool> MoveCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
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
	protected Func<bool> CoverEnemyCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
							player, gameManager.GetEnemies(player), card);
	}
	protected Func<bool> CoverOwnCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
							player, new List<Player>() { player }, card);
	}
	protected Func<bool> CoverPlayersCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
				player, gameManager.Players, card);
	}
	protected List<Player> TotemFilter(List<Player> players) {
		return PlayersWithoutCardFilter(players, "totem");
	}
	protected Func<bool> DropPlayersCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(gameManager.Players), card);
	}
	protected Func<bool> DropEnemyCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(gameManager.GetEnemies(player)), card);
	}
	protected Func<bool> DropOwnCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(new List<Player>() { player }), card);
	}
	protected Func<bool> MixPlayersCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, gameManager.Players, card);
	}
	protected Func<bool> MixOwnCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, new List<Player>() { player }, card);
	}
	protected Func<bool> MixEnemyCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, gameManager.GetEnemies(player), card);
	}
	protected Func<bool> MoveToOtherSelectEffect(Player player, Card card) {
		return MoveCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
							player, gameManager.Players, card);
	}
	protected Func<bool> TakeFromEnemySelectEffect(Player player, Card card) {
		return TakeAwayCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
			player, gameManager.GetEnemies(player), card);
	}
}
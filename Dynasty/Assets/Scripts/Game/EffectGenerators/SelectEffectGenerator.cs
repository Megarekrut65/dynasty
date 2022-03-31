using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using CardEffect = System.Func<bool>;

public abstract class SelectEffectGenerator : SimpleEffectsGenerator {
	private SelectManager selectManager;
	
	public SelectEffectGenerator(GameDependencies dependencies,
	CardManager cardManager, Table table, AnimationEffectGenerator anim)
		: base(dependencies, cardManager, table, anim) {
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
	protected CardEffect TakeCardFromDropSelectEffect(Player player, Card card, bool callNext = true) {
		return () => {
			card.needSelect = true;
			dependencies.cameraMove.Stop = true;
			dependencies.scrollManager.ViewSetActive(true);
			behaviour.StartCoroutine(TakeFromDrop(player, card, callNext));
			return CardEffect(player, card, false)();
		};
	}
	private IEnumerator TakeFromDrop(Player player, Card card, bool callNext) {
		var cards = table.GetRCardsInDrop();
		foreach (var c in cards) {
			cardManager.CreateCard(c);
			dependencies.scrollManager.AddToScroll(c.obj);
		}
		selectManager.SelectCard(player, cards, id => {
			behaviour.StartCoroutine(TakeAction(id, player, callNext));
		}, dependencies.playerManager.IsPlayer(player));
		yield return null;
	}
	private IEnumerator TakeAction(int id, Player player, bool callNext) {
		var take = table.RemoveCardFromDrop(id);
		var cards = table.GetRCardsInDrop();
		foreach (var c in cards) {
			cardManager.DeleteCardFromTable(c);
		}
		yield return new WaitForSeconds(1f);
		Action after = () => {
			CallNext(callNext)();
			dependencies.cameraMove.Stop = false;
			dependencies.scrollManager.ViewSetActive(false);
		};
		if (take != null) {
			anim.AddCardToPlayerAnimated(take, player, after);
		} else after();
		yield return null;
	}
	protected CardEffect DropCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
								Card card, bool callNext = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectDrop(predicate, player, players,
				id => {
					logger.LogAction(player, id, "dropped");
					var drop = table.RemoveCardFromPlayer(id);
					if (drop != null)
						anim.DropCardAnimated(drop, CallNext(callNext));
					else CallNext(callNext)();
				}, dependencies.playerManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect CoverCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
							Card card, bool callNext = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectCover(predicate, player, players,
				id => {
					logger.LogAction(player, id, "covered");
					var cover = table.FindCardInPlayers(id);
					if (cover != null)
						anim.CoverCardAnimated(cover, card, CallNext(callNext));
					else CardEffect(player, card)();
				}, dependencies.playerManager.IsPlayer(player));
			return true;
		};
	}
	protected CardEffect MixCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
								Card card, bool callNext = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMix(predicate, player, players,
				id => {
					logger.LogAction(player, id, "mixed");
					var mix = table.RemoveCardFromPlayer(id);
					if (mix != null)
						anim.InsertPlayerCardToDeskAnimated(mix, CallNext(callNext));
					else CallNext(callNext)();
				}, dependencies.playerManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect TakeAwayCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool callNext = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMove(predicate, player, players,
				id => {
					logger.LogAction(player, id, "took-away");
					var take = table.RemoveCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, player, CallNext(callNext));
					else CallNext(callNext)();
				}, dependencies.playerManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect MoveCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
									Card card, bool callNext = true) {
		return () => {
			card.needSelect = true;
			selectManager.SelectMoveToOther(predicate, player, players,
				(id, pl) => {
					var take = table.RemoveCardFromPlayer(id);
					if (take != null)
						anim.AddCardToPlayerAnimated(take, pl, () => {
							logger.LogAction(player, id, "moved");
							CallNext(callNext)();
						});
					else CallNext(callNext)();
				}, dependencies.playerManager.IsPlayer(player));
			return CardEffect(player, card, false)();
		};
	}
	protected CardEffect CoverEnemyCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
							player, dependencies.playerManager.GetEnemies(player), card);
	}
	protected CardEffect CoverOwnCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
							player, new List<Player>() { player }, card);
	}
	protected CardEffect CoverPlayersCardSelectEffect(Player player, Card card) {
		return CoverCardSelectEffect(GameAction.GetAllFilter(CardFunctions.COVER),
				player, dependencies.playerManager.Players, card);
	}
	protected List<Player> TotemFilter(List<Player> players) {
		return PlayersWithoutCardFilter(players, "totem");
	}
	protected CardEffect DropPlayersCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(dependencies.playerManager.Players), card);
	}
	protected CardEffect DropEnemyCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(dependencies.playerManager.GetEnemies(player)), card);
	}
	protected CardEffect DropOwnCardSelectEffect(Player player, Card card) {
		return DropCardSelectEffect(GameAction.GetAllFilter(CardFunctions.DROP),
					player, TotemFilter(new List<Player>() { player }), card);
	}
	protected CardEffect MixPlayersCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, dependencies.playerManager.Players, card);
	}
	protected CardEffect MixOwnCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, new List<Player>() { player }, card);
	}
	protected CardEffect MixEnemyCardSelectEffect(Player player, Card card) {
		return MixCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MIX),
			player, dependencies.playerManager.GetEnemies(player), card);
	}
	protected CardEffect MoveToOtherSelectEffect(Player player, Card card) {
		return MoveCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
							player, dependencies.playerManager.Players, card);
	}
	protected CardEffect TakeFromEnemySelectEffect(Player player, Card card) {
		return TakeAwayCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
			player, dependencies.playerManager.GetEnemies(player), card);
	}
}
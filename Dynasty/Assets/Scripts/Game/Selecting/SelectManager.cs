using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager {
	private Table table;

	public SelectManager(Table table) {
		this.table = table;
	}
	public void SelectAllMix(Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select("mix", owner, other, select, canClick, GetFilter("mix"));
	}
	public void SelectAllMove(Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select("move", owner, other, select, canClick, GetFilter("move"));
	}
	public void SelectAllDrop(Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select("drop", owner, other, select, canClick, GetFilter("drop"));
	}
	public void SelectAllCover(Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select("cover", owner, other, select, canClick, GetFilter("cover"));
	}
	private Predicate<Card> GetFilter(string icon) {
		return (card) => GetIconValue(icon, card) != "no";
	}
	private void Select(string icon, Player owner, List<Player> other,
						Action<int> select, bool canClick, Predicate<Card> filter) {
		var playersDesk = GetDesks(icon, other, filter);
		if (IsEmpty(playersDesk)) {
			select(-1);
			return;
		}
		Color color = owner.GetColor();
		foreach (var item in playersDesk) {
			foreach (var card in item.Value) {
				CardManager.CreateOutline(card, color);
				var selectClick = card.obj.AddComponent<SelectClick>();
				selectClick.Id = card.id;
				selectClick.Select = (id) => {
					select(id);
				};
				selectClick.CanClick = canClick;
			}
		}
	}
	private Dictionary<Player, List<Card>> GetDesks(string icon, List<Player> other, Predicate<Card> filter) {
		var playersDesk = table.FilterAllCardsInPlayers(filter);
		foreach (var item in playersDesk) {
			if (other.Contains(item.Key))
				LeavePriority(icon, item.Value);
			else item.Value.Clear();
		}
		return playersDesk;
	}
	private bool IsEmpty(Dictionary<Player, List<Card>> playersDesk) {
		foreach (var item in playersDesk) {
			if (item.Value.Count != 0) {
				return false;
			}
		}
		return true;
	}
	private void LeavePriority(string icon, List<Card> cards) {
		var card = cards.Find((c) => GetIconValue(icon, c) == "yes");
		if (card != null) {
			cards.RemoveAll((c) => GetIconValue(icon, c) != "yes");
		}
	}
	private string GetIconValue(string icon, Card card) {
		switch (icon) {
			case "mix": return card.data.mix;
			case "move": return card.data.move;
			case "cover": return card.data.cover;
			case "drop": return card.data.drop;
			default: return "";
		}
	}
}
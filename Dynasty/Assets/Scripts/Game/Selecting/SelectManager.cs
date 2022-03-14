using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager {
	private Table table;
	private static List<SelectData> lastSelecting = new List<SelectData>();
	private static string lastType = "";
	public static List<SelectData> LastSelecting {
		get {
			return lastSelecting;
		}
	}
	public static string LastType {
		get {
			return lastType;
		}
	}
	public SelectManager(Table table) {
		this.table = table;
	}
	public void SelectMix(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		lastType = CardFunctions.MIX;
		Select(lastType, owner, other, select, canClick, filter);
	}
	public void SelectMove(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		lastType = CardFunctions.MOVE;
		Select(lastType, owner, other, select, canClick, filter);
	}
	public void SelectDrop(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		lastType = CardFunctions.DROP;
		Select(lastType, owner, other, select, canClick, filter);
	}
	public void SelectCover(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		lastType = CardFunctions.COVER;
		Select(lastType, owner, other, select, canClick, filter);
	}

	private void Select(string icon, Player owner, List<Player> other,
						Action<int> select, bool canClick, Predicate<Card> filter) {
		lastSelecting.Clear();
		var playersDesk = GetDesks(icon, other, filter);
		if (IsEmpty(playersDesk)) {
			select(-1);
			return;
		}
		Color color = owner.GetColor();
		foreach (var item in playersDesk) {
			foreach (var card in item.Value) {
				var outline = CardManager.CreateOutline(card, color);
				var selectClick = card.obj.AddComponent<SelectClick>();
				lastSelecting.Add(new SelectData(outline, selectClick, card));
				selectClick.Id = card.id;
				selectClick.Select = (id) => {
					ClearSelecting();
					select(id);
				};
				selectClick.CanClick = canClick;
			}
		}
	}
	private void ClearSelecting() {
		foreach (var item in lastSelecting) {
			MonoBehaviour.Destroy(item.outline);
			MonoBehaviour.Destroy(item.selectClick);
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
		var card = cards.Find((c) => CardFunctions.GetIconValue(icon, c) == "yes");
		if (card != null) {
			cards.RemoveAll((c) => CardFunctions.GetIconValue(icon, c) != "yes");
		}
	}

}
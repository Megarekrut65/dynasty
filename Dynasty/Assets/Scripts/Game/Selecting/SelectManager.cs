using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager {
	private Table table;
	private static SelectData selectData = new SelectData();
	public static SelectData SelectData {
		get {
			return selectData;
		}
	}
	public SelectManager(Table table) {
		this.table = table;
	}
	public void SelectCard(Player owner, List<Card> cards, Action<int> select, bool canClick) {
		Select(CardFunctions.MOVE, owner, null, null, canClick, null, false);
		selectData.toOwner = true;
		if (cards.Count == 0) {
			select(-1);
			return;
		}
		foreach (var card in cards) {
			AddClick(card, card.obj, owner.GetColor(),
				selectData.selectingCards, select, canClick, card.id, owner);
		}
	}
	public void SelectMix(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select(CardFunctions.MIX, owner, other, select, canClick, filter);
	}
	public void SelectMove(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select(CardFunctions.MOVE, owner, other, select, canClick, filter);
	}
	public void SelectMoveToOther(Predicate<Card> filter, Player owner, List<Player> other, Action<int, Player> select, bool canClick) {
		if (GetDesks(CardFunctions.MOVE, other, filter)[owner].Count == 0) {
			selectData.Clear();
			select(-1, owner);
			return;
		}
		Select(CardFunctions.MOVE, owner, other, (i) => { }, canClick, filter, false);
		SelectPlayer(CardFunctions.MOVE, owner, other, select, canClick, filter);
	}
	public void SelectDrop(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select(CardFunctions.DROP, owner, other, select, canClick, filter);
	}
	public void SelectCover(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
		Select(CardFunctions.COVER, owner, other, select, canClick, filter);
	}

	private void Select(string type, Player owner, List<Player> other,
						Action<int> select, bool canClick, Predicate<Card> filter, bool toOwner = true) {
		selectData.Clear();
		selectData.lastType = type;
		selectData.toOwner = toOwner;
		if (toOwner) SelectCard(type, owner, other, select, canClick, filter);
	}
	public void SelectEnemy(Player owner, List<Player> other, Action<Player> select, bool canClick) {
		Action<int> selectPlayer = (i) => {
			ClearSelectingPlayers();
			select(other[i]);
		};
		AddPlayerClick(owner, other, selectPlayer, canClick);
	}
	private void SelectPlayer(string icon, Player owner, List<Player> other,
						Action<int, Player> select, bool canClick, Predicate<Card> filter) {
		Action<int> selectPlayer = (i) => {
			ClearSelectingPlayers();
			SelectCard(icon, other[i], new List<Player>() { owner }, (id) => select(id, other[i]), canClick, filter);
		};
		AddPlayerClick(owner, other, selectPlayer, canClick);
	}
	private void AddPlayerClick(Player owner, List<Player> other, Action<int> selectPlayer, bool canClick) {
		for (int i = 0; i < other.Count; i++) {
			var pl = other[i];
			if (!pl.Equals(owner)) {
				var label = pl.Label;
				AddClick(label, label, owner.GetColor(), selectData.selectingPlayers, selectPlayer, canClick, i, pl);
			}
		}
	}
	private void SelectCard(string icon, Player owner, List<Player> other,
						Action<int> select, bool canClick, Predicate<Card> filter) {
		var playersDesk = GetDesks(icon, other, filter);
		if (IsEmpty(playersDesk)) {
			select(-1);
			return;
		}
		Color color = owner.GetColor();
		foreach (var item in playersDesk) {
			foreach (var card in item.Value) {
				AddClick(card, card.obj, color, selectData.selectingCards, select, canClick, card.id, item.Key);
			}
		}
	}
	private void AddClick<ObjectType>(ObjectType obj, GameObject gameObject, Color color,
				List<SelectObjectData<ObjectType>> list, Action<int> select, bool canClick, int id, Player owner) {
		var outline = CardManager.CreateOutline(gameObject, color);
		var selectClick = gameObject.AddComponent<SelectClick>();
		list.Add(new SelectObjectData<ObjectType>(outline, selectClick, obj, owner));
		selectClick.Id = id;
		selectClick.Select = (i) => {
			ClearSelectingCards();
			select(i);
		};
		selectClick.CanClick = canClick;
	}
	private void ClearSelectingCards() {
		foreach (var item in selectData.selectingCards) {
			MonoBehaviour.Destroy(item.outline);
			MonoBehaviour.Destroy(item.selectClick);
		}

	}
	private void ClearSelectingPlayers() {
		foreach (var item in selectData.selectingPlayers) {
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
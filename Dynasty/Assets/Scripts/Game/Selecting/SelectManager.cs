using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Class that creates all parts of selecting
/// </summary>
public class SelectManager {
    private Table table;
    private FullScreenCardMaker fullScreenCardMaker;
    public static SelectData SelectData { get; } = new SelectData();

    public SelectManager(Table table, FullScreenCardMaker fullScreenCardMaker) {
        this.table = table;
        this.fullScreenCardMaker = fullScreenCardMaker;
    }
    public void SelectMix(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select, bool canClick) {
        Select(CardFunctions.MIX, owner, other, select, canClick, filter);
    }
    public void SelectMove(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select,
        bool canClick) {
        Select(CardFunctions.MOVE, owner, other, select, canClick, filter);
    }
    public void SelectMoveToOther(Predicate<Card> filter, Player owner, List<Player> other, Action<int, Player> select,
        bool canClick) {
        if (GetDesks(CardFunctions.MOVE, other, filter)[owner].Count == 0) {
            SelectData.Clear();
            select(-1, owner);
            return;
        }

        Select(CardFunctions.MOVE, owner, other, i => { }, canClick, filter, false);
        SelectPlayer(CardFunctions.MOVE, owner, other, select, canClick, filter);
    }
    public void SelectDrop(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select,
        bool canClick) {
        Select(CardFunctions.DROP, owner, other, select, canClick, filter);
    }
    public void SelectCover(Predicate<Card> filter, Player owner, List<Player> other, Action<int> select,
        bool canClick) {
        Select(CardFunctions.COVER, owner, other, select, canClick, filter);
    }

    private void Select(string type, Player owner, List<Player> other,
        Action<int> select, bool canClick, Predicate<Card> filter, bool toOwner = true) {
        SelectData.Clear();
        SelectData.lastType = type;
        SelectData.toOwner = toOwner;
        if (toOwner) SelectCard(type, owner, other, select, canClick, filter);
    }
    private void SelectPlayer(string icon, Player owner, List<Player> other,
        Action<int, Player> select, bool canClick, Predicate<Card> filter) {
        Action<int> selectPlayer = i => {
            ClearSelectingPlayers();
            SelectCard(icon, other[i], new List<Player>() {owner}, id => select(id, other[i]), canClick, filter);
        };
        AddPlayerClick(owner, other, selectPlayer, canClick);
    }
    private void AddPlayerClick(Player owner, List<Player> other, Action<int> selectPlayer, bool canClick) {
        for (int i = 0; i < other.Count; i++) {
            var pl = other[i];
            if (!pl.Equals(owner)) {
                var label = pl.Label;
                AddClick(label, label, owner.GetColor(),
                    SelectData.selectingPlayers, selectPlayer, canClick, i, pl, true, null);
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
                AddClick(card, card.obj, color, SelectData.selectingCards, select, 
                    canClick, card.id, item.Key, false, card.key);
            }
        }
    }
    private void AddClick<TObjectType>(TObjectType obj, GameObject gameObject, Color color,
        List<SelectObjectData<TObjectType>> list, Action<int> select, bool canClick,
        int id, Player owner, bool isPlayer, string key) {
        var outline = CardController.CreateOutline(gameObject, color);
        var selectClick = gameObject.AddComponent<SelectClick>();
        list.Add(new SelectObjectData<TObjectType>(outline, selectClick, obj, owner));
        selectClick.Id = id;
        Action<int> newSelect = i => {
            ClearSelectingCards();
            select(i);
        };
        SelectClickEffect selectClickEffect = new SelectClickEffect(id, isPlayer, newSelect, 
            canClick, gameObject.transform);
        if (canClick && !isPlayer) {
            ButtonEffect buttonEffect = new ButtonEffect(gameObject.transform, null, null, true, 3);
            selectClick.Down = e => {buttonEffect.Down(); };
            selectClick.Up = eventData => {
                buttonEffect.Up();
                fullScreenCardMaker.Make(selectClickEffect, key, color);
            };
            return;
        }
        selectClick.Down = eventData=>selectClickEffect.Down(eventData);
        selectClick.Up = eventData=>selectClickEffect.Up(eventData);
    }
    private void ClearSelectingCards() {
        foreach (var item in SelectData.selectingCards) {
            Object.Destroy(item.outline);
            Object.Destroy(item.selectClick);
        }
    }
    private void ClearSelectingPlayers() {
        foreach (var item in SelectData.selectingPlayers) {
            Object.Destroy(item.outline);
            Object.Destroy(item.selectClick);
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
        var card = cards.Find(c => CardFunctions.GetIconValue(icon, c) == "yes");
        if (card != null) {
            cards.RemoveAll(c => CardFunctions.GetIconValue(icon, c) != "yes");
        }
    }
}
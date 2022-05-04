using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using JetBrains.Annotations;
using UnityEngine;

public class OnlineEntityController:EntityController {
    private DatabaseReference playerReference;
    private DatabaseReference gameReference;
    private Queue<Action> saved = new Queue<Action>();
    [CanBeNull]
    private Card card = null;
    public OnlineEntityController(Player player, 
        GameDependencies dependencies, Table table, Func<Card> takeCard, DatabaseReference playerReference) 
        : base(player, dependencies, table, takeCard) {
        this.playerReference = playerReference;
        gameReference = playerReference.Child(GameKeys.GAME_STATE);
        gameReference.Child(GameKeys.IS_CLICK).ValueChanged += PlayerClick;
        gameReference.Child(GameKeys.CARD_CLICK).ValueChanged += PlayerCardClick;
        gameReference.Child(GameKeys.SELECTING).Child(GameKeys.SELECT_PLAYER).ValueChanged += DoSelectPlayer;
        gameReference.Child(GameKeys.SELECTING).Child(GameKeys.SELECT_CARD).ValueChanged += DoSelectCard;
    }
    private void DoSelectCard(object sender, ValueChangedEventArgs e) {
        object obj =  e.Snapshot.Value;
        if(obj == null) return;
        Play(() => {
            if(!(card is {needSelect: true})) return;
            int cardIndex = (int) obj;
            var list = SelectManager.SelectData.selectingCards;
            if(list.Count >= cardIndex) return;
            Click(list[cardIndex].selectClick);
        });
    }
    private void DoSelectPlayer(object sender, ValueChangedEventArgs e) {
        object obj =  e.Snapshot.Value;
        if(obj == null) return;
        Play(() => {
            if(!(card is {needSelect: true}) || !SelectManager.SelectData.toOwner) return;
            int playerIndex = (int) obj;
            var list = SelectManager.SelectData.selectingPlayers;
            if(list.Count >= playerIndex) return;
            Click(list[playerIndex].selectClick);
        });
    }
    private void PlayerCardClick(object sender, ValueChangedEventArgs e) {
        object obj = e.Snapshot.Value;
        if(obj == null || !(bool)obj) return;
        Play(() => {
            if(card == null) return;
            Click(card.obj.GetComponent<CardClick>());
        });
    }
    private void PlayerClick(object sender, ValueChangedEventArgs e) {
        object obj = e.Snapshot.Value;
        if(obj == null || !(bool)obj) return;
        gameReference.Child(GameKeys.IS_CLICK).SetValueAsync(false);
        Play(() => card = takeCard());
    }
    protected override IEnumerator InevitableEnd(Card card) {
        throw new NotImplementedException();
    }
    protected override SelectObjectData<GameObject> SelectPlayer() {
        throw new NotImplementedException();
    }
    protected override SelectObjectData<Card> SelectCard() {
        throw new NotImplementedException();
    }
    protected override void Next() {
        foreach (var action in saved) {
            action();
        }
    }
    private bool OwnRound() {
        Player next = dependencies.roundManager.WhoIsNextPlayer();
        return next.Equals(player);
    }
    private void Play(Action action) {
        if (OwnRound()) action();
        else saved.Enqueue(action);
    }
}
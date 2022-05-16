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
            int cardId = Convert.ToInt32(obj);
            var list = SelectManager.SelectData.selectingCards;
            var cardObj = list.Find((c) => c.obj.id == cardId);
            if(cardObj == null || cardObj.selectClick == null) return;
            dependencies.cameraMove.StartCoroutine(Click(cardObj.selectClick));
        });
    }
    private void DoSelectPlayer(object sender, ValueChangedEventArgs e) {
        object obj =  e.Snapshot.Value;
        if(obj == null) return;
        Play(() => {
            if(!(card is {needSelect: true}) || !SelectManager.SelectData.toOwner) return;
            int playerIndex = Convert.ToInt32(obj);
            var list = SelectManager.SelectData.selectingPlayers;
            if(list.Count >= playerIndex) return;
            dependencies.cameraMove.StartCoroutine(Click(list[playerIndex].selectClick));
        });
    }
    private void PlayerCardClick(object sender, ValueChangedEventArgs e) {
        object obj = e.Snapshot.Value;
        if(obj == null || !(bool)obj) return;
        gameReference.Child(GameKeys.CARD_CLICK).SetValueAsync(false);
        Play(() => {
            if(card == null||card.obj == null) return;
            dependencies.cameraMove.StartCoroutine(Click(card.obj.GetComponent<CardClick>()));
        });
    }
    private void PlayerClick(object sender, ValueChangedEventArgs e) {
        object obj = e.Snapshot.Value;
        if(obj == null || !(bool)obj) return;
        gameReference.Child(GameKeys.IS_CLICK).SetValueAsync(false);
        Play(() => card = takeCard(), true);
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
        Player now = dependencies.roundManager.WhoIsNow();
        now ??= dependencies.roundManager.WhoIsNextPlayer();
        return player.Equals(now);
    }
    private bool NextOwnRound() {
        Player next = dependencies.roundManager.WhoIsNextPlayer();
        return player.Equals(next);
    }
    private void Play(Action action, bool checkNext = false) {
        if (checkNext && NextOwnRound() || !checkNext && OwnRound()) {
            action();
        }
        else saved.Enqueue(action);
    }
}
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
    private MonoBehaviour behaviour;
    [CanBeNull]
    private Card card = null;
    public OnlineEntityController(Player player, 
        GameDependencies dependencies, Table table, Func<Card> takeCard, DatabaseReference playerReference) 
        : base(player, dependencies, table, takeCard) {
        behaviour = dependencies.cameraMove;
        this.playerReference = playerReference;
        gameReference = playerReference.Child(GameKeys.GAME_STATE);
        gameReference.Child(GameKeys.IS_CLICK).ValueChanged += PlayerClick;
        gameReference.Child(GameKeys.CARD_CLICK).ValueChanged += PlayerCardClick;
        gameReference.Child(GameKeys.SELECTING).Child(GameKeys.SELECT_PLAYER).ValueChanged += DoSelectPlayer;
        gameReference.Child(GameKeys.SELECTING).Child(GameKeys.SELECT_CARD).ValueChanged += DoSelectCard;
        gameReference.Child(GameKeys.AVOID_END).ValueChanged += AvoidEnd;
    }
    private void AvoidEnd(object sender, ValueChangedEventArgs e) {
        object obj = e.Snapshot.Value;
        if(obj == null || !(bool)obj) return;
        gameReference.Child(GameKeys.AVOID_END).SetValueAsync(false);
        Play(() => {
            var avoid = table.FindCardInPlayer(player, "avoid-inevitable");
            if(avoid == null) return;
            behaviour.StartCoroutine(Click(avoid.obj.GetComponent<CardClick>()));
        });
    }
    private void DoSelectCard(object sender, ValueChangedEventArgs e) {
        object obj = e.Snapshot.Value;
        if(obj == null) return;
        Debug.Log("Do select");
        Play(() => {
            gameReference.Child(GameKeys.SELECTING).Child(GameKeys.SELECT_CARD).SetValueAsync(null);
            Debug.Log("Do select play");
            if(!(card is {needSelect: true})) return;
            int cardId = Convert.ToInt32(obj);
            Debug.Log("Do select card id " + cardId);
            var list = SelectManager.SelectData.selectingCards;
            var cardObj = list.Find((c) => c.obj.id == cardId);
            Debug.Log("Do select card" + cardObj);
            if(cardObj == null || cardObj.selectClick == null) return;
            Debug.Log("Do select coroutine");
            behaviour.StartCoroutine(Click(cardObj.selectClick));
        });
    }
    private void DoSelectPlayer(object sender, ValueChangedEventArgs e) {
        object obj =  e.Snapshot.Value;
        if(obj == null) return;
        Debug.Log("Do select player");
        Play(() => {
            Debug.Log("Do select player play");
            gameReference.Child(GameKeys.SELECTING).Child(GameKeys.SELECT_PLAYER).SetValueAsync(null);
            Debug.Log("Select player card" + card + " need " + card?.needSelect);
            Debug.Log("select player select manager to owner: " + SelectManager.SelectData.toOwner);
            if(!(card is {needSelect: true}) || SelectManager.SelectData.toOwner) return;
            int playerId = Convert.ToInt32(obj);
            Debug.Log("Do select player id " + playerId);
            var list = SelectManager.SelectData.selectingPlayers;
            var pl = list.Find((p) => p.selectClick.Id == playerId);
            Debug.Log("Do select player name " + pl?.owner.Nickname);
            if(pl == null) return;
            behaviour.StartCoroutine(Click(pl.selectClick));
        });
    }
    private void PlayerCardClick(object sender, ValueChangedEventArgs e) {
        object obj = e.Snapshot.Value;
        if(obj == null || !(bool)obj) return;
        gameReference.Child(GameKeys.CARD_CLICK).SetValueAsync(false);
        Play(() => {
            if(card == null||card.obj == null) return;
            behaviour.StartCoroutine(Click(card.obj.GetComponent<CardClick>()));
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
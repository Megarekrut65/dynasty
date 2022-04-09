using System;
using System.Collections;
using Firebase.Database;
using UnityEngine;

public class OnlineController:Controller {
    private DatabaseReference playerReference;
    
    public OnlineController(Player player, 
        GameDependencies dependencies, Table table, Func<Card> takeCard, DatabaseReference playerReference) 
        : base(player, dependencies, table, takeCard) {
        this.playerReference = playerReference;
    }
    protected override IEnumerator InevitableEnd(Card card) {
        throw new NotImplementedException();
    }
    protected override SelectObjectData<GameObject> SelectPlayer() {
        throw new NotImplementedException();
    }
    protected override IEnumerator WaitForClick() {
        while (true) {
            yield return null;
            var value = playerReference.Child("game").GetValueAsync().Result.Value;
            if(value != null) break;
        }
    }
    protected override SelectObjectData<Card> SelectCard() {
        throw new NotImplementedException();
    }
}
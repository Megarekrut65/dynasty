using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BotController : EntityController {
    public BotController(Player player, GameDependencies dependencies,
        Table table, CardFullScreenMaker cardFullScreenMaker, Func<Card> takeCard)
        : base(player, dependencies, table, cardFullScreenMaker, takeCard) {
    }

    protected override IEnumerator InevitableEnd(Card card) {
        Card avoid = table.FindCardInPlayer(player, "avoid-inevitable");
        if (avoid != null) yield return Click(avoid.obj.GetComponent<CardClick>());
        else yield return Click(card.obj.GetComponent<CardClick>());
    }
    protected override SelectObjectData<GameObject> SelectPlayer() {
        return SelectManager.SelectData.selectingPlayers[Random.Range(0,
            SelectManager.SelectData.selectingPlayers.Count)];
    }
}
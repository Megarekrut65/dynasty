using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that count coins of all players at the end of game
/// </summary>
public class CoinsCounter {
    private GameDependencies gameDependencies;
    private AnimationEffectGenerator anim;
    private EffectLogger logger;
    private int playerCount = 0;
    private Action end;

    public CoinsCounter(GameDependencies gameDependencies, AnimationEffectGenerator anim, EffectLogger logger) {
        this.gameDependencies = gameDependencies;
        this.anim = anim;
        this.logger = logger;
    }
    public void StartCounting(Table table, Action endCount) {
        this.end = endCount;
        table.CountRCardCoins(item => { gameDependencies.cameraMove.StartCoroutine(CountCoins(item)); });
    }
    private IEnumerator CountCoins(KeyValuePair<Player, List<Card>> item) {
        var player = item.Key;
        var cards = item.Value;
        int amount = 0;
        foreach (var card in cards) {
            amount += card.data.amount;
            anim.CountAmountAnimated(player, card);
            yield return new WaitForSeconds(0.5f);
        }

        logger.LogCoins(player, amount);
        yield return new WaitForSeconds(0.5f);
        logger.LogTotal(player);
        cards.Clear();
        playerCount++;
        if (playerCount == gameDependencies.playerManager.GetEntityCount()) {
            yield return new WaitForSeconds(2f);
            end();
        }
    }
}
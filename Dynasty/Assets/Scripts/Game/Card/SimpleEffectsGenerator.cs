using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimpleEffectsGenerator
{
    protected GameManager gameManager;
    protected CardManager cardManager;
    protected Table table;
    protected CardAnimationManager animationManager;

    public SimpleEffectsGenerator(GameManager gameManager,
        CardManager cardManager, Table table, CardAnimationManager animationManager)
    {
        this.gameManager = gameManager;
        this.cardManager = cardManager;
        this.table = table;
        this.animationManager = animationManager;
    }
    protected Func<bool> CardMoreEffect(int more, Player player, Card card, bool other = true)
    {
        return () =>
        {
            gameManager.AddCount(more);
            if (other) return OtherEffect(player, card)();
            return true;
        };
    }
    protected Func<bool> DropEffect(string key, Player player, Card card, bool other = true)
    {
        return () =>
        {
            Card take = table.GetCardFromPlayer(key);
            if (take != null)
            {
                animationManager.PlayDropCard(take.obj, () =>
                {
                    cardManager.DeleteCardFromTable(take);
                    table.DropCard(take);
                    return true;
                });
            }
            if (other) return OtherEffect(player, card)();
            return true;
        };
    }
    protected Func<bool> TakeAwayCardEffect(string key, Player player, Card card, bool other = true)
    {
        return () =>
        {
            Card take = table.GetCardFromPlayer(key);
            if (take != null)
            {
                animationManager.PlayCardHideShowAnimation(take.obj, () =>
                {
                    player.AddCard(take);
                    table.AddCardToPlayer(player, take);
                    return true;
                }, () => { return true; });
            }
            if (other) return OtherEffect(player, card)();
            return true;
        };
    }
    protected Func<bool> MoveToCardEffect(string key, Player player, Card card, bool other = true)
    {
        return () =>
        {
            Player owner = player;
            Player with = table.GetPlayerWithCard(key);
            if (with != null) owner = with;
            if (other) return OtherEffect(owner, card)();
            return true;
        };
    }
    protected Func<bool> MixEffect(string key, Player player, Card card, bool other = true)
    {
        return () =>
        {
            Card mix = table.GetCardFromPlayer(key);
            if (mix != null)
            {
                animationManager.PlayCardHideAnimation(mix.obj, () =>
                {
                    InsertCard(mix);
                    return true;
                });

            }
            if (other) return OtherEffect(player, card)();
            return true;
        };
    }
    protected Func<bool> OtherEffect(Player player, Card card, bool callNext = true)
    {
        return () =>
        {
            //Use effect
            //after effect

            if (card.data.type == "A")
            {
                player.AddCard(card);
                if (card.data.mix == "yes")
                {
                    animationManager.PlayCardToDesk(card.obj, () =>
                    {
                        InsertCard(card);
                        if (callNext) gameManager.StartCoroutine(Next());
                        return true;
                    });
                }
                else
                {
                    animationManager.PlayDropCard(card.obj, () =>
                    {
                        cardManager.DeleteCardFromTable(card);
                        table.DropCardFromPlayer(player, card);
                        if (callNext) gameManager.StartCoroutine(Next());
                        return true;
                    });
                }
            }
            else
            {
                animationManager.PlayCardHideShowAnimation(card.obj, () =>
                {
                    player.AddCard(card);
                    table.AddCardToPlayer(player, card);
                    return true;
                }, () =>
                {
                    if (callNext) gameManager.StartCoroutine(Next());
                    return true;
                });
            }
            return true;
        };
    }
    protected void InsertCard(Card card)
    {
        cardManager.DeleteCardFromTable(card);
        table.InsertToDesk(card);
    }
    IEnumerator Next()
    {
        yield return new WaitForSeconds(1f);
        gameManager.CallNext();
    }
}
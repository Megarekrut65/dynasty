using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimpleEffectsGenerator
{
    protected GameManager gameManager;
    protected CardManager cardManager;
    protected Table table;

    public SimpleEffectsGenerator(GameManager gameManager, CardManager cardManager, Table table)
    {
        this.gameManager = gameManager;
        this.cardManager = cardManager;
        this.table = table;
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
                cardManager.DeleteCardFromTable(take);
                table.DropCard(take);
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
                player.AddCard(take);
                table.AddCardToPlayer(player, take);
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
                cardManager.DeleteCardFromTable(mix);
                table.InsertToDesk(mix);
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
            player.AddCard(card);
            if (card.data.type == "A")
            {
                cardManager.DeleteCardFromTable(card);
                if (card.data.mix == "yes") table.InsertToDesk(card);
                else table.DropCardFromPlayer(player, card);
            }
            else table.AddCardToPlayer(player, card);
            if (callNext) gameManager.CallNext();
            return true;
        };
    }
}
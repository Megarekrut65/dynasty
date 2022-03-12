using System.Collections;
using System;
using UnityEngine;


public class CardBot
{
    private Player player;
    private GameManager gameManager;
    private Table table;
    private Func<Card> takeCard;

    public CardBot(Player player, GameManager gameManager, Table table, Func<Card> takeCard)
    {
        this.player = player;
        this.gameManager = gameManager;
        this.table = table;
        this.takeCard = takeCard;
        gameManager.Next += Next;
    }
    public void Next()
    {
        Player next = gameManager.GetNextPlayer();
        if (next.nickname != this.player.nickname) return;
        gameManager.StartCoroutine(ClickOnCard());
    }
    IEnumerator ClickOnCard()
    {
        Card card = takeCard();
        yield return new WaitForSeconds(2f);
        if (card != null)
        {
            if (card.key == "inevitable-end")
            {
                Card avoid = table.FindCardInPlayer(player, "avoid-inevitable");
                if (avoid != null) yield return Click(avoid);
                else yield return Click(card);
            }
            else yield return Click(card);
        }
    }
    IEnumerator Click(Card card)
    {
        CardClick cardClick = card.obj.GetComponent<CardClick>();
        cardClick.OnPointerDown(null);
        yield return new WaitForSeconds(0.1f);
        cardClick.OnPointerUp(null);
    }
}
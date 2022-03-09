using System.Collections;
using System;
using UnityEngine;


public class CardBot {
    private Player player;
    private GameManager gameManager;
    private Func<Card> takeCard;

    public CardBot(Player player, GameManager gameManager, Func<Card> takeCard){
        this.player = player;
        this.gameManager = gameManager;
        this.takeCard = takeCard;
        gameManager.Next += Next;
    }
    public void Next(){
        Player next = gameManager.GetNextPlayer();
        if(next.nickname != this.player.nickname) return;
        gameManager.StartCoroutine(ClickOnCard());
    }
    IEnumerator ClickOnCard(){
        yield return new WaitForSeconds(0.5f);
        Card card = takeCard();
        if(card != null){
            ButtonScript buttonScript = card.obj.GetComponent<ButtonScript>();
            CardClick cardClick = card.obj.GetComponent<CardClick>();
            buttonScript.OnPointerDown(null);
            yield return new WaitForSeconds(0.5f);
            buttonScript.OnPointerUp(null);
            cardClick.OnPointerUp(null);
        }
    }
    
}
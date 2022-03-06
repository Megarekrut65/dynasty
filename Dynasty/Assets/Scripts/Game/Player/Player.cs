using System.Collections.Generic;
using UnityEngine;

public class Player {
    private string nickname;
    private Desk desk;
    private int coins = 0;
    
    public Player(string nickname, Desk desk){
        this.desk = desk;
        this.nickname = nickname;
        desk.SetName(nickname);
    }
    public void AddCard(Card card){
        if(card.data.type == "A"){
            coins += card.data.amount;
            desk.SetCoins(coins);
        }
        else desk.AddCard(card);
    }
}
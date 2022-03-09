using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
public class Table {
    private List<Card> desk = new List<Card>();
    private List<Card> drop = new List<Card>();
    private Dictionary<Player, List<Card>> playerDesk = new Dictionary<Player, List<Card>>();
    private Card current;
    public Card Current{
        get{
            return current;
        }
        set{
            current = value;
        }
    }
    public Table(List<Player> players){
        foreach(var player in players){
            playerDesk.Add(player, new List<Card>());
        }
        List<Card> data = new List<Card>();
        var map = LocalizationManager.instance.map.CardMap;
        //Card end = null;
        foreach(var item in map){
            /*if(item.Key == "inevitable-end"){
                end = new Card(item.Value, item.Key);
                continue;
            }*/
            for(int i = 0; i < item.Value.count; i++){
                data.Add(new Card(item.Value, item.Key));
            }
        }
        System.Random rnd = new System.Random();
        while(data.Count != 0){
            int index = rnd.Next(data.Count);
            var item = data[index];
            desk.Add(item);
            data.Remove(item);
        }
        //if(end != null) desk.Insert(desk.Count - 10, end);
    }
    public void InsertToDesk(Card card){
        desk.Insert(UnityEngine.Random.Range(0, desk.Count - 1), card);
    }
    public Card TakeCardFromDesk(){
        current = desk[desk.Count - 1];
        desk.Remove(current);
        
        return current;
    }
    public void AddCardToPlayer(Player player, Card card){
        playerDesk[player].Add(card);
    }
    public void DropCard(Player player, Card card){
        playerDesk[player].Remove(card);
        drop.Add(card);
    }
    public void CountRCardCoins(Func<KeyValuePair<Player, List<Card>>, bool> coroutine){
        foreach(var item in playerDesk){
            coroutine(item);
        }
    }
}
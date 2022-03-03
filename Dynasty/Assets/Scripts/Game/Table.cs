using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class Table {
    private Stack<Card> desk = new Stack<Card>();
    private List<Card> drop = new List<Card>();
    private Dictionary<Player, List<Card>> playerDesk = new Dictionary<Player, List<Card>>();
    public Table(List<Player> players){
        foreach(var player in players){
            playerDesk.Add(player, new List<Card>());
        }
        List<Card> data = new List<Card>();
        var map = LocalizationManager.instance.map.CardMap;
        foreach(var item in map){
            data.Add(new Card(item.Value, item.Key));
        }
        System.Random rnd = new System.Random();
        while(data.Count != 0){
            int index = rnd.Next(data.Count);
            var item = data[index];
            desk.Push(item);
            data.Remove(item);
        }
        foreach(var card in desk){
            UnityEngine.Debug.Log(card.data);
        }
    }
    public Card TakeCardFromDesk(){
        var item = desk.Pop();
        return item;
    }
}
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
public class Table
{
    private List<Card> desk = new List<Card>();
    private List<Card> drop = new List<Card>();
    private Dictionary<Player, List<Card>> playerDesk = new Dictionary<Player, List<Card>>();
    private Card current;
    public Card Current
    {
        get
        {
            return current;
        }
        set
        {
            current = value;
        }
    }
    public Table(List<Player> players)
    {
        foreach (var player in players)
        {
            playerDesk.Add(player, new List<Card>());
        }
        List<Card> data = new List<Card>();
        var map = LocalizationManager.instance.map.CardMap;
        List<Card> toEnd = new List<Card>();
        Card keys = new Card(map["step-ahead"], "step-ahead");
        foreach (var item in map)
        {
            if (item.Key == "gold-mountain")
            {
                for (int i = 0; i < item.Value.count; i++) toEnd.Add(new Card(item.Value, item.Key));
                continue;
            }
            //if (item.Value.type == "A" && item.Value.mix == "none") continue;
            for (int i = 0; i < item.Value.count; i++)
            {
                data.Add(new Card(item.Value, item.Key));
            }
        }
        System.Random rnd = new System.Random();
        while (data.Count != 0)
        {
            int index = rnd.Next(data.Count);
            var item = data[index];
            desk.Add(item);
            data.Remove(item);
        }
        foreach (var end in toEnd)
        {
            desk.Insert(desk.Count - 6, end);
        }
        if (keys.data != null)
        {
            desk.Insert(desk.Count - 7, keys);
        }
    }
    public void InsertToDesk(Card card)
    {
        //desk.Insert(desk.Count - 5, card);
        desk.Insert(UnityEngine.Random.Range(0, desk.Count - 1), card);
    }
    public void InsertToDeskFromPlayer(Player player, Card card)
    {
        playerDesk[player].Remove(card);
        desk.Insert(UnityEngine.Random.Range(0, desk.Count - 1), card);
    }
    public Card TakeCardFromDesk()
    {
        current = desk[desk.Count - 1];
        desk.Remove(current);

        return current;
    }
    public void AddCardToPlayer(Player player, Card card)
    {
        playerDesk[player].Add(card);
    }
    public void DropCard(Card card)
    {
        drop.Add(card);
    }
    public void DropCardFromPlayer(Player player, Card card)
    {
        playerDesk[player].Remove(card);
        drop.Add(card);
    }
    public void CountRCardCoins(Func<KeyValuePair<Player, List<Card>>, bool> coroutine)
    {
        foreach (var item in playerDesk)
        {
            coroutine(item);
        }
    }
    public Card FindCardInPlayer(string key)
    {
        var item = FindCard(key);
        if (item != null)
        {
            return item.Item1;
        }
        return null;
    }
    public Card GetCardFromPlayer(string key)
    {
        var item = FindCard(key);
        if (item != null)
        {
            item.Item2.Remove(item.Item1);
            return item.Item1;
        }
        return null;
    }
    private Tuple<Card, List<Card>> FindCard(string key)
    {
        foreach (var item in playerDesk)
        {
            Card it = item.Value.Find(card => card.key == key);
            if (it != null)
            {
                return new Tuple<Card, List<Card>>(it, item.Value);
            }
        }
        return null;
    }
    public Player GetPlayerWithCard(string key)
    {
        foreach (var item in playerDesk)
        {
            Card it = item.Value.Find(card => card.key == key);
            if (it != null)
            {
                return item.Key;
            }
        }
        return null;
    }
    public List<Card> GetAllCardFromPlayer(Player player, Predicate<Card> comparator)
    {
        List<Card> cards = playerDesk[player].FindAll(comparator);
        playerDesk[player].RemoveAll(comparator);
        return cards;
    }
}
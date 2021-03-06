using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Class that manage all cards in game
/// </summary>
[Serializable]
public class Table {
    [SerializeField]
    private List<Card> desk;
    [SerializeField]
    private List<Card> drop = new List<Card>();
    private Dictionary<Player, List<Card>> playerDesk = new Dictionary<Player, List<Card>>();
    [SerializeField]
    private Card current;
    public Card Current {
        get => current;
        set => current = value;
    }
    [SerializeField]
    private bool nextFromDown = false;
    public bool NextFromDown {
        set => nextFromDown = value;
    }
    private Random rnd;
    public Table(List<Player> players) {
        foreach (var player in players) {
            playerDesk.Add(player, new List<Card>());
        }

        rnd = new Random(LocalStorage.GetValue(LocalStorage.DESK_SEED, 0));
        desk = DeskGenerator.Generate(rnd);
    }
    public void AddPlayer(Player player) {
        playerDesk.Add(player, new List<Card>());
    }
    public void InsertToDesk(Card card) {
        desk.Insert(rnd.Next(desk.Count - 1), card);
    }
    private void UnderCard(Card card, List<Card> list) {
        if (card.underCard != null) {
            list.Add(card.underCard);
        }
    }
    public void InsertToDeskFromPlayer(Player player, Card card) {
        playerDesk[player].Remove(card);
        UnderCard(card, playerDesk[player]);
        InsertToDesk(card);
    }
    public Card TakeCardFromDesk() {
        int index = nextFromDown ? 0 : desk.Count - 1;
        nextFromDown = false;
        current = desk[index];
        desk.Remove(current);

        return current;
    }
    public void AddCardToPlayer(Player player, Card card) {
        playerDesk[player].Add(card);
    }
    public void CoverCard(Card under, Card top) {
        var underPlayer = FindPlayerWithCard(under.id);
        playerDesk[underPlayer].Remove(under);
        top.underCard = under;
        playerDesk[underPlayer].Add(top);
    }
    public void DropCard(Card card) {
        drop.Add(card);
    }
    public void DropCardFromPlayer(Player player, Card card) {
        playerDesk[player].Remove(card);
        UnderCard(card, playerDesk[player]);
        drop.Add(card);
    }
    public void CountRCardCoins(Action<KeyValuePair<Player, List<Card>>> coroutine) {
        foreach (var item in playerDesk) {
            coroutine(item);
        }
    }
    public Card FindCardInPlayers(int id) {
        return FindCardInPlayers(card => card.id == id);
    }
    public Card FindCardInPlayers(string key) {
        return FindCardInPlayers(card => card.key == key);
    }
    private Card FindCardInPlayers(Predicate<Card> predicate) {
        var item = FindCard(predicate);
        return item?.Item1;
    }
    public Card FindCardInPlayer(Player player, string key) {
        return playerDesk[player].Find(card => card.key == key);
    }
    public Card FindCardInPlayer(Player player, int id) {
        return playerDesk[player].Find(card => card.id == id);
    }
    public Card RemoveCardFromPlayer(string key) {
        return RemoveCardFromPlayer(card => card.key == key);
    }
    public Card RemoveCardFromPlayer(int id) {
        return RemoveCardFromPlayer(card => card.id == id);
    }
    public Card RemoveCardFromPlayer(Predicate<Card> predicate) {
        var item = FindCard(predicate);
        if (item != null) {
            item.Item2.Remove(item.Item1);
            UnderCard(item.Item1, item.Item2);
            return item.Item1;
        }

        return null;
    }
    private Tuple<Card, List<Card>> FindCard(Predicate<Card> predicate) {
        foreach (var item in playerDesk) {
            Card it = item.Value.Find(predicate);
            if (it != null) {
                return new Tuple<Card, List<Card>>(it, item.Value);
            }
        }

        return null;
    }
    public Player FindPlayerWithCard(string key) {
        return FindPlayerWithCard(card => card.key == key);
    }
    public Player FindPlayerWithCard(int id) {
        return FindPlayerWithCard(card => card.id == id);
    }
    private Player FindPlayerWithCard(Predicate<Card> predicate) {
        foreach (var item in playerDesk) {
            Card it = item.Value.Find(predicate);
            if (it != null) {
                return item.Key;
            }
        }

        return null;
    }
    public List<Card> RemoveAllCardsFromPlayer(Player player, Predicate<Card> comparator) {
        List<Card> cards = playerDesk[player].FindAll(comparator);
        foreach (var card in cards) {
            UnderCard(card, playerDesk[player]);
        }

        playerDesk[player].RemoveAll(comparator);
        return cards;
    }
    public List<Card> FindAllCardsInPlayer(Player player, Predicate<Card> comparator) {
        return playerDesk[player].FindAll(comparator);
    }
    public List<Card> RemoveAllCardsFromPlayers(Player owner, Predicate<Card> comparator) {
        List<Card> res = new List<Card>();
        foreach (var item in playerDesk) {
            if (item.Key.Equals(owner)) continue;
            List<Card> cards = item.Value.FindAll(comparator);
            foreach (var card in cards) {
                UnderCard(card, item.Value);
            }

            item.Value.RemoveAll(comparator);
            res.AddRange(cards);
        }

        return res;
    }
    public Dictionary<Player, List<Card>> FilterAllCardsInPlayers(Predicate<Card> comparator) {
        Dictionary<Player, List<Card>> res = new Dictionary<Player, List<Card>>();
        foreach (var item in playerDesk) {
            res[item.Key] = item.Value.FindAll(comparator);
        }

        return res;
    }
    public List<Card> GetRCardsInDrop() {
        return drop.Where(card => card.data.type == "R").ToList();
    }
    public Card RemoveCardFromDrop(int id) {
        var card = drop.Find(card => card.id == id);
        if (card == null) return null;
        drop.Remove(card);
        return card;
    }
}
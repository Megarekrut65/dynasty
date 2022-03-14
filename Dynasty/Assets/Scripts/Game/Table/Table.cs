using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
public class Table {
	private List<Card> desk;
	private List<Card> drop = new List<Card>();
	private Dictionary<Player, List<Card>> playerDesk = new Dictionary<Player, List<Card>>();
	private Card current;
	public Card Current {
		get {
			return current;
		}
		set {
			current = value;
		}
	}
	public Table(List<Player> players) {
		foreach (var player in players) {
			playerDesk.Add(player, new List<Card>());
		}
		desk = DeskGenerator.Generate((card) => {
			return card.key == "robin-hood";
		}, 6);
	}
	public void InsertToDesk(Card card) {
		desk.Insert(UnityEngine.Random.Range(0, desk.Count - 1), card);
	}
	public void InsertToDeskFromPlayer(Player player, Card card) {
		playerDesk[player].Remove(card);
		desk.Insert(UnityEngine.Random.Range(0, desk.Count - 1), card);
	}
	public Card TakeCardFromDesk() {
		current = desk[desk.Count - 1];
		desk.Remove(current);

		return current;
	}
	public void AddCardToPlayer(Player player, Card card) {
		playerDesk[player].Add(card);
	}
	public void DropCard(Card card) {
		drop.Add(card);
	}
	public void DropCardFromPlayer(Player player, Card card) {
		playerDesk[player].Remove(card);
		drop.Add(card);
	}
	public void CountRCardCoins(Func<KeyValuePair<Player, List<Card>>, bool> coroutine) {
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
		if (item != null) {
			return item.Item1;
		}
		return null;
	}
	public Card FindCardInPlayer(Player player, string key) {
		return playerDesk[player].Find(card => card.key == key);
	}
	public Card GetCardFromPlayer(string key) {
		return GetCardFromPlayer(card => card.key == key);
	}
	public Card GetCardFromPlayer(int id) {
		return GetCardFromPlayer(card => card.id == id);
	}
	public Card GetCardFromPlayer(Predicate<Card> predicate) {
		var item = FindCard(predicate);
		if (item != null) {
			item.Item2.Remove(item.Item1);
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
	public Player GetPlayerWithCard(string key) {
		foreach (var item in playerDesk) {
			Card it = item.Value.Find(card => card.key == key);
			if (it != null) {
				return item.Key;
			}
		}
		return null;
	}
	public List<Card> GetAllCardsFromPlayer(Player player, Predicate<Card> comparator) {
		List<Card> cards = playerDesk[player].FindAll(comparator);
		playerDesk[player].RemoveAll(comparator);
		return cards;
	}
	public List<Card> FindAllCardsInPlayer(Player player, Predicate<Card> comparator) {
		return playerDesk[player].FindAll(comparator);
	}
	public List<Card> GetAllCardsFromPlayers(Player owner, Predicate<Card> comparator) {
		List<Card> res = new List<Card>();
		foreach (var item in playerDesk) {
			if (item.Key.Equals(owner)) continue;
			List<Card> cards = item.Value.FindAll(comparator);
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
}
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager {
	private List<Player> players;
	public List<Player> Players {
		get {
			return players;
		}
	}
	private int playersCount;
	public int PlayersCount {
		get {
			return playersCount;
		}
	}
	private int botsCount;
	public int BotsCount {
		get {
			return botsCount;
		}
	}
	private List<Controller> bots;

	public PlayerManager() {
		this.playersCount = 1;
		this.botsCount = 5;
		string[] nicknames = new string[6];
		this.players = new List<Player>();
		this.bots = new List<Controller>();
		for (int i = 0; i < nicknames.Length; i++) {
			nicknames[i] = "Player" + UnityEngine.Random.Range(0, 1000);
		}
		nicknames[0] = "You";
		for (int i = 0; i < playersCount + botsCount; i++) {
			players.Add(new Player(nicknames[i]));
		}
	}
	public bool IsPlayer(Player player) {
		int index = players.FindIndex((pl) => pl.Equals(player));
		return (index < playersCount);
	}
	public List<Player> GetEnemies(Player player) {
		return players.Where(p => !p.Equals(player)).ToList();
	}
	public void CreateBots(GameDependencies dependencies, Table table, Func<Card> takeCardFromDesk) {
		for (int i = playersCount; i < players.Count; i++) {
			bots.Add(new RandomBotController(players[i], dependencies, table, takeCardFromDesk));
		}
	}
}
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

	public PlayerManager(Desk[] desks) {
		this.playersCount = PlayerPrefs.GetInt("player-count");
		this.botsCount = 0;
		if (Convert.ToBoolean(PlayerPrefs.GetString("enable-bots")))
			this.botsCount = PlayerPrefs.GetInt("bot-count");
		this.players = new List<Player>();
		this.bots = new List<Controller>();
		var nicknames = CreateNames();
		for (int i = 0; i < playersCount + botsCount; i++) {
			players.Add(new Player(nicknames[i], desks[i]));
		}
		for (int i = playersCount + botsCount; i < desks.Length; i++) {
			desks[i].SetActive(false);
		}
		players.Sort((pl1, pl2) => pl1.Order - pl2.Order);
	}
	private string[] CreateNames() {
		string[] nicknames = new string[6];
		nicknames[0] = "You";
		for (int i = 1; i < playersCount; i++) {
			nicknames[i] = "Player" + i;
		}
		for (int i = playersCount; i < playersCount + botsCount; i++) {
			nicknames[i] = "Bot" + UnityEngine.Random.Range(0, 1000);
		}
		return nicknames;
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
			bots.Add(
				ControllerFactory.CreateController(PlayerPrefs.GetString("difficulty-bots"),
					players[i], dependencies, table, takeCardFromDesk));
		}
	}
}
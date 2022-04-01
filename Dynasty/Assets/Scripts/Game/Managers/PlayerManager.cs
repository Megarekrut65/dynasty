using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager {
	public List<Player> Players { get; }
	private int playersCount;
	private int botsCount;
	private List<Controller> bots;

	public PlayerManager(Desk[] desks) {
		this.playersCount = PlayerPrefs.GetInt(PrefabsKeys.PLAYER_COUNT);
		this.botsCount = 0;
		if (Convert.ToBoolean(PlayerPrefs.GetString(PrefabsKeys.ENABLE_BOTS)))
			this.botsCount = PlayerPrefs.GetInt(PrefabsKeys.BOT_COUNT);
		this.Players = new List<Player>();
		this.bots = new List<Controller>();
		var nicknames = CreateNames();
		for (int i = 0; i < playersCount + botsCount; i++) {
			Players.Add(new Player(nicknames[i], desks[i]));
		}
		for (int i = playersCount + botsCount; i < desks.Length; i++) {
			desks[i].SetActive(false);
		}
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
		return bots.FindIndex(bot => bot.Player.Equals(player)) == -1;
	}
	public List<Player> GetEnemies(Player player) {
		return Players.Where(p => !p.Equals(player)).ToList();
	}
	public void CreateBots(GameDependencies dependencies, Table table, Func<Card> takeCardFromDesk) {
		for (int i = playersCount; i < Players.Count; i++) {
			bots.Add(
				ControllerFactory.CreateController(PlayerPrefs.GetString(PrefabsKeys.DIFFICULTY_BOTS),
					Players[i], dependencies, table, takeCardFromDesk));
		}
		Players.Sort((pl1, pl2) => pl1.Order - pl2.Order);
	}
}
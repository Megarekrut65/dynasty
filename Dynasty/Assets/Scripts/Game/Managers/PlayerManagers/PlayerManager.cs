using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class PlayerManager {
	public List<Player> Players { get; }
	protected int playersCount;
	protected List<Controller> controllers;
	private Desk[] desks;
	private string controllerType;
	
	public PlayerManager(Desk[] desks, int playersCount, int deskBegin, string controllerType) {
		this.desks = desks;
		Array.Sort(desks, (d1, d2) => d1.Order - d2.Order);
		this.playersCount = playersCount;
		this.controllerType = controllerType;
		this.Players = new List<Player>();
		this.controllers = new List<Controller>();
		for (int i = deskBegin; i < desks.Length; i++) {
			desks[i].SetActive(false);
		}
	}
	public abstract bool IsPlayer(Player player);
	public List<Player> GetEnemies(Player player) {
		return Players.Where(p => !p.Equals(player)).ToList();
	}
	public Player AddController(string name, GameDependencies dependencies, Table table, Func<Card> takeCardFromDesk) {
		Players.Add(new Player(name, desks[Players.Count]));
		var player = Players[Players.Count - 1];
		controllers.Add(
			ControllerFactory.CreateController(controllerType,
				player, dependencies, table, takeCardFromDesk));
		return player;
	}
	public abstract int GetEntityCount();
	public int GetPlayersCount() {
		return playersCount;
	}
}
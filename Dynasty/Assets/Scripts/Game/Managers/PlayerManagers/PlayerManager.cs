using System;
using System.Collections.Generic;
using System.Linq;

public abstract class PlayerManager {
    public List<Player> Players { get; }
    protected int playersCount;
    protected List<EntityController> controllers;
    private Desk[] desks;
    private string controllerType;

    protected PlayerManager(Desk[] desks, int playersCount, int deskBegin, string controllerType) {
        this.desks = desks;
        Array.Sort(desks, 0, deskBegin, new DeskComparer());
        this.playersCount = playersCount;
        this.controllerType = controllerType;
        this.Players = new List<Player>();
        this.controllers = new List<EntityController>();
        for (int i = deskBegin; i < desks.Length; i++) {
            desks[i].SetActive(false);
        }
    }
    public abstract bool IsPlayer(Player player);
    public List<Player> GetEnemies(Player player) {
        return Players.Where(p => !p.Equals(player)).ToList();
    }
    public Player AddController(string name, GameDependencies dependencies, Table table, Func<Card> takeCardFromDesk) {
        var player = new Player(name, desks[Players.Count], (Players.Count + 1).ToString());
        Players.Add(player);
        controllers.Add(
            EntityControllerFactory.CreateController(controllerType,
                player, dependencies, table, takeCardFromDesk));
        return player;
    }
    public abstract int GetEntityCount();
    public int GetPlayersCount() {
        return playersCount;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnlinePlayerManager: PlayerManager{
    private Player current;

    public OnlinePlayerManager(Desk[] desks, Player current, int playerCount)
        :base(desks,playerCount, playerCount, GameMode.ONLINE.ToString()) {
        this.current = current;
        Players.Add(current);
    }
    public override bool IsPlayer(Player player) {
        return current.Equals(player);
    }
    public override int GetEntityCount() {
        return playersCount;
    }
}
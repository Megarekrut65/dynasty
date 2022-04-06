using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OfflinePlayerManager:PlayerManager {
    private int botsCount;

    public OfflinePlayerManager(Desk[] desks, int playerCount, int botCount):
    base(desks, playerCount, playerCount + botCount, 
        PlayerPrefs.GetString(PrefabsKeys.DIFFICULTY_BOTS)) {
        this.botsCount = botCount;
        var nicknames = CreateNames();
        for (int i = 0; i < playersCount; i++) {
            Players.Add(new Player(nicknames[i], desks[i]));
        }
        Players.Sort((pl1, pl2) => pl1.Order - pl2.Order);
    }
    private string[] CreateNames() {
        string[] nicknames = new string[6];
        nicknames[0] = "You";
        for (int i = 1; i < playersCount; i++) {
            nicknames[i] = "Player" + i;
        }
        return nicknames;
    }
    public override bool IsPlayer(Player player) {
        return controllers.FindIndex(bot => bot.Player.Equals(player)) == -1;
    }
    public override int GetEntityCount() {
        return playersCount + botsCount;
    }
}
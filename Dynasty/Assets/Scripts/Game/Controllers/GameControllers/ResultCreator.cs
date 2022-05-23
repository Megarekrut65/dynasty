using Firebase.Database;
using UnityEngine;

public class ResultCreator {
    private PlayerManager playerManager;
    private GameCloser gameCloser;
    
    public ResultCreator(PlayerManager playerManager, GameCloser gameCloser) {
        this.playerManager = playerManager;
        this.gameCloser = gameCloser;
    }
    public void MakeWinResult() {
        PlayerPrefs.SetString(LocalStorage.GAME_RESULT, Translator.Translate("you win"));
    }
    public void MakeResult() {
        PlayerPrefs.SetString(LocalStorage.GAME_RESULT, gameCloser.GameClosed?GameClosedResult():GameLeaveResult());
    }
    private string GameLeaveResult() {
        return Translator.Translate("you lose");
    }
    private string GameClosedResult() {
        var players = playerManager.Players;
        players.Sort((player1, player2) => player2.Coins.CompareTo(player1.Coins));
        string res = "\t1. "+Win(players[0]);
        for (int i = 1; i < players.Count; i++) {
            res += "\t"+(i+1)+". "+(players[i].Coins < players[0].Coins ? Lose(players[i]) : Win(players[i]));
        }

        return res;
    }
    private string Win(Player player) {
        return Translator.Translate("player") +" "+ player.Nickname 
               + " "+ Translator.Translate("win with " + player.Coins + " coins!") + "\n";
    }
    private string Lose(Player player) {
        return Translator.Translate("player") +" "+ player.Nickname 
               + " "+ Translator.Translate("lose with " + player.Coins + " coins!")+ "\n";
    }
}
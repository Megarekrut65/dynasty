using UnityEngine;

public class EffectLogger {
	private GameManager gameManager;
	private Table table;
	public EffectLogger(GameManager gameManager, Table table) {
		this.gameManager = gameManager;
		this.table = table;
	}
	public void LogGot(Player player, Card card) {
		gameManager.TranslatedLog(
				$"{player.nickname} got card \'{card.data.name}\'");
	}
	public void LogSomeCards(Player player) {
		gameManager.TranslatedLog(
				$"{player.nickname} got some cards");
	}
	public void LogAction(Player player, string key, string action) {
		var owner = table.FindPlayerWithCard(key);
		if (owner == null) return;
		LogAction(player, owner, table.FindCardInPlayer(owner, key), action);
	}
	public void LogAction(Player player, int id, string action) {
		var owner = table.FindPlayerWithCard(id);
		if (owner == null) return;
		LogAction(player, owner, table.FindCardInPlayer(owner, id), action);
	}
	private void LogAction(Player player, Player owner, Card card, string action) {
		if (card != null) {
			LogEffect(player, card, owner, action);
		}
	}
	private void LogEffect(Player owner, Card card, Player target, string action) {
		string word = action == "moved" ? "to" : "of";
		gameManager.TranslatedLog(
			$"{owner.nickname} {action} card \'{card.data.name}\' {word} {target.nickname}");
	}
	public void LogCoins(Player player, int coins) {
		string word = coins >= 0 ? "got" : "lost";
		gameManager.TranslatedLog(
			$"{player.nickname} {word} {coins} coins");
	}
	public void LogTotal(Player player) {
		gameManager.TranslatedLog(
			$"{player.nickname} has total {player.Coins} coins");
	}
	public void GameOver() {
		gameManager.TranslatedLog("game-over");
	}
	public void LogInsert(Player player, string name) {
		gameManager.TranslatedLog(
			$"{player.nickname} mixed card \'{name}\'");
	}
}
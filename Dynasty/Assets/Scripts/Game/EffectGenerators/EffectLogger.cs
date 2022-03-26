using UnityEngine;

public class EffectLogger {
	private GameLogger logger;
	private Table table;

	public EffectLogger(GameLogger logger, Table table) {
		this.logger = logger;
		this.table = table;
	}
	public void LogGot(Player player, Card card) {
		logger.TranslatedLog(
				$"{player.nickname} got card \'{card.data.name}\'");
	}
	public void LogSomeCards(Player player) {
		logger.TranslatedLog(
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
		logger.TranslatedLog(
			$"{owner.nickname} {action} card \'{card.data.name}\' {word} {target.nickname}");
	}
	public void LogCoins(Player player, int coins) {
		string word = coins >= 0 ? "got" : "lost";
		logger.TranslatedLog(
			$"{player.nickname} {word} {coins} coins");
	}
	public void LogTotal(Player player) {
		logger.TranslatedLog(
			$"{player.nickname} has total {player.Coins} coins");
	}
	public void GameOver() {
		logger.TranslatedLog("game-over");
	}
	public void LogInsert(Player player, string name) {
		logger.TranslatedLog(
			$"{player.nickname} mixed card \'{name}\'");
	}
}
using UnityEngine;
using System.Collections.Generic;

public class TableManager : MonoBehaviour {
	[SerializeField]
	private GameManager gameManager;
	[SerializeField]
	private CardAnimationManager animationManager;
	private GameDependencies dependencies;
	private CardDependencies cardDependencies;
	private GameController gameController;
	
	private void Start() {
		dependencies = gameManager.GameDependencies;
		cardDependencies = gameManager.CardDependencies;
		gameController = dependencies.gameController;
		var players = dependencies.playerManager.Players;
		if (GameModeFunctions.IsMode(GameMode.OFFLINE)) {
			for (int i = dependencies.playerManager.GetPlayersCount(); i < dependencies.playerManager.GetEntityCount(); i++) {
				var player = dependencies.playerManager.AddController("Bot" + i, dependencies, cardDependencies.Table, TakeCardFromDesk);
				cardDependencies.Table.AddPlayer(player);
			}
			gameManager.StartGame();
		}
	}
	public bool PlayerRound() {
		return dependencies.playerManager.IsPlayer(dependencies.roundManager.WhoIsNextPlayer());
	}
	public Card TakeCardFromDesk() {
		if (gameController.GameOver|| dependencies.roundManager.Pause) return null;
		dependencies.roundManager.Pause = true;
		var card = cardDependencies.Table.TakeCardFromDesk();
		if (card.key == "inevitable-end") {
			gameController.GameOver = true;
		}
		cardDependencies.Controller.CreateCard(card);
		animationManager.PlayCardFromDeskAnimation(card.obj, () => {
			if (!gameController.GameOver)
				dependencies.bigCardManager.MakeBig(card.obj);
		});
		Player next = dependencies.roundManager.WhoIsNextPlayer();
		dependencies.logger.TranslatedLog($"{next.Nickname} took card \'{card.data.name}\' from desk");
		cardDependencies.Controller.AddClickToCard(card,
			cardDependencies.EffectsGenerator.GetEffect(dependencies.roundManager.GetTheNextPlayer(), card),
			next.GetColor(),
			dependencies.playerManager.IsPlayer(next));

		return card;
	}
}
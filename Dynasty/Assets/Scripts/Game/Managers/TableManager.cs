using UnityEngine;
using System.Collections.Generic;

public class TableManager : MonoBehaviour {
	[SerializeField]
	private GameManager gameManager;
	
	private void Start() {
		var dependencies = gameManager.GameDependencies;
		var cardDependencies = gameManager.CardDependencies;
		if (GameModeFunctions.IsMode(GameMode.OFFLINE)) {
			for (int i = dependencies.playerManager.GetPlayersCount(); i < dependencies.playerManager.GetEntityCount(); i++) {
				var player = dependencies.playerManager.AddController(
					"Bot" + i, dependencies, cardDependencies.Table, gameManager.CardTaker.TakeCardFromDesk);
				cardDependencies.Table.AddPlayer(player);
			}
			gameManager.StartGame();
		}
	}
}
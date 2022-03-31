using UnityEngine;
using System.Collections.Generic;

public class TableManager : MonoBehaviour {
	[SerializeField]
	private GameObject cardObject;
	[SerializeField]
	private GameManager gameManager;
	[SerializeField]
	private CardAnimationManager animationManager;
	[Header("Card place")]
	[SerializeField]
	private GameObject container;
	[Header("")]
	[SerializeField]
	private Table table;
	private CardManager cardManager;
	private EffectsGenerator effectsGenerator;
	private AnimationEffectGenerator animationEffectGenerator;
	private GameDependencies dependencies;

	private void Start() {
		cardManager = new CardManager(container, cardObject);
		dependencies = gameManager.Dependencies;
		var players = dependencies.playerManager.Players;
		table = new Table(players);
		animationEffectGenerator = new AnimationEffectGenerator(cardManager, table, animationManager);
		effectsGenerator = new EffectsGenerator(gameManager, dependencies, cardManager, table, animationEffectGenerator);
		AddStartCards(players);
		dependencies.playerManager.CreateBots(dependencies, table, TakeCardFromDesk);
		dependencies.logger.TranslatedLog("game-begun");
		dependencies.roundManager.CallNextPlayer();
	}
	private void AddStartCards(List<Player> players) {
		string avoidKey = "avoid-inevitable";
		CardData avoid = LocalizationManager.Instance.GetCard(avoidKey);
		foreach (var player in players) {
			Card card = new Card(avoid, avoidKey);
			cardManager.CreateCard(card);
			cardManager.AddClickToCard(card,
				effectsGenerator.GetEffect(player, card), new Vector4(0f, 0f, 0f, 0f),
				dependencies.playerManager.IsPlayer(player));
			animationEffectGenerator.AddCardToPlayerAnimated(card, player, () => { });
		}
	}
	public bool PlayerRound() {
		return dependencies.playerManager.IsPlayer(dependencies.roundManager.WhoIsNextPlayer());
	}
	public Card TakeCardFromDesk() {
		if (gameManager.GameOver || dependencies.roundManager.Pause) return null;
		dependencies.roundManager.Pause = true;
		var card = table.TakeCardFromDesk();
		if (card.key == "inevitable-end") {
			gameManager.GameOver = true;
		}
		cardManager.CreateCard(card);
		animationManager.PlayCardFromDeskAnimation(card.obj, () => {
			if (!gameManager.GameOver)
				dependencies.bigCardManager.MakeBig(card.obj);
		});
		Player next = dependencies.roundManager.WhoIsNextPlayer();
		dependencies.logger.TranslatedLog($"{next.nickname} took card \'{card.data.name}\' from desk");
		cardManager.AddClickToCard(card,
			effectsGenerator.GetEffect(dependencies.roundManager.GetTheNextPlayer(), card),
			next.GetColor(),
			dependencies.playerManager.IsPlayer(next));

		return card;
	}
}
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DependenciesManager {
	[SerializeField]
	private Desk[] playerDesks = new Desk[6];
	public Desk[] PlayerDesks => playerDesks;
	[Header("Big Card")]
	[SerializeField]
	private Toggle bigCard;
	[Header("Camera Controller")]
	[SerializeField]
	private CameraMove cameraMove;
	[Header("Logging")]
	[SerializeField]
	private GameLogger logger;
	[Header("Scroll View")]
	[SerializeField]
	private GameObject contentObject;
	[SerializeField]
	private GameObject scrollRect;
	[SerializeField]
	private GameObject view;
	private GameDependencies dependencies;

	public GameDependencies GetGameDependencies() {
		if (dependencies == null) {
			PlayerManager playerManager = null;
			int playerCount = PrefabsKeys.GetValue(PrefabsKeys.PLAYER_COUNT, 2);
			if (GameModeFunctions.IsMode(GameMode.OFFLINE)) {
				playerManager = new OfflinePlayerManager(playerDesks,
					playerCount,
					Convert.ToBoolean(PrefabsKeys.GetValue(PrefabsKeys.ENABLE_BOTS))
						? PrefabsKeys.GetValue(PrefabsKeys.BOT_COUNT, 0)
						: 0);
			} else {
				playerManager = new OnlinePlayerManager(playerDesks,playerCount);
			}
			var roundManager = new RoundManager(playerManager.Players);
			dependencies = new GameDependencies {
				bigCardManager = new BigCardManager(scrollRect, bigCard),
				scrollManager = new ScrollManager(scrollRect.GetComponent<ScrollRect>(), contentObject, view),
				playerManager = playerManager,
				roundManager = roundManager,
				cameraMove = cameraMove,
				logger = logger,
				gameController = new GameController(logger, roundManager)
			};
		}

		return dependencies;
	}
	[Header("Card manager dependencies")]
	[SerializeField]
	private GameObject cardContainer;
	[SerializeField]
	private GameObject cardObject;
	[SerializeField]
	private CardAnimationManager cardAnimationManager;
	private CardDependencies cardDependencies;
	public CardDependencies GetCardDependencies() {
		return cardDependencies ??= new CardDependencies(GetGameDependencies(), cardContainer, cardObject, cardAnimationManager);
	}
	private CardTaker cardTaker;
	public CardTaker GetCardTaker() {
		return cardTaker ??= new CardTaker(GetGameDependencies(), GetCardDependencies());
	}
}
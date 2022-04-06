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

	public GameDependencies GetDependencies() {
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
				int index = Convert.ToInt32(PrefabsKeys.GetValue(PrefabsKeys.PLAYER_KEY));
				var player = new Player(PrefabsKeys.GetValue(PrefabsKeys.PLAYER_NAME), playerDesks[index - 1]);
				playerManager = new OnlinePlayerManager(playerDesks, player, playerCount);
			}
			dependencies = new GameDependencies {
				bigCardManager = new BigCardManager(scrollRect, bigCard),
				scrollManager = new ScrollManager(scrollRect.GetComponent<ScrollRect>(), contentObject, view),
				playerManager = playerManager,
				roundManager = new RoundManager(playerManager.Players),
				cameraMove = cameraMove,
				logger = logger
			};
		}

		return dependencies;
	}
}
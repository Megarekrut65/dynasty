using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DependenciesManager {
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
			var playerManager = new PlayerManager();
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
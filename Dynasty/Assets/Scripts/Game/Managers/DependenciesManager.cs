using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
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

    private PlayerManager GetPlayerManager() {
        int playerCount = LocalStorage.GetValue(LocalStorage.PLAYER_COUNT, 2);
        if (GameModeFunctions.IsMode(GameMode.OFFLINE)) {
            return new OfflinePlayerManager(playerDesks,
                playerCount,
                Convert.ToBoolean(LocalStorage.GetValue(LocalStorage.ENABLE_BOTS))
                    ? LocalStorage.GetValue(LocalStorage.BOT_COUNT, 0)
                    : 0);
        }

        return new OnlinePlayerManager(playerDesks, playerCount);
    }
    public GameDependencies GetGameDependencies() {
        if (dependencies == null) {
            PlayerManager playerManager = GetPlayerManager();
            var roundManager = new RoundManager(playerManager.Players);
            dependencies = new GameDependencies {
                scrollManager = new ScrollManager(scrollRect.GetComponent<ScrollRect>(), contentObject, view),
                playerManager = playerManager,
                roundManager = roundManager,
                cameraMove = cameraMove,
                logger = logger,
                gameStarter = new GameStarter(logger, roundManager),
                gameCloser = new GameCloser()
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
    [SerializeField]
    private CardFullScreenMaker cardFullScreenMaker;
    private CardDependencies cardDependencies;
    public CardDependencies GetCardDependencies() {
        return cardDependencies ??=
            new CardDependencies(GetGameDependencies(), cardContainer, cardObject, cardAnimationManager, cardFullScreenMaker);
    }
    private CardTaker cardTaker;
    public CardTaker GetCardTaker() {
        return cardTaker ??= new CardTaker(GetGameDependencies(), GetCardDependencies());
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private RoomUI roomUI;
    [SerializeField]
    private DependenciesManager dependenciesManager = new DependenciesManager();

    public GameDependencies GameDependencies => dependenciesManager.GetGameDependencies();
    private CardDependencies CardDependencies => dependenciesManager.GetCardDependencies();
    public CardTaker CardTaker => dependenciesManager.GetCardTaker();
    private GameController gameController;

    private void Start() {
        var mode = GameModeFunctions.IsMode(GameMode.OFFLINE)
            ? GameMode.OFFLINE
            : GameMode.ONLINE;
        if (mode == GameMode.ONLINE) {
            gameController = new OnlineGameController(GameDependencies, CardDependencies, CardTaker,
                dependenciesManager.PlayerDesks, roomUI);
            return;
        }

        gameController = new OfflineGameController(GameDependencies, CardDependencies, CardTaker);
    }
    public void Leave() {
        gameController.Leave();
    }
}
/// <summary>
/// Class that start game
/// </summary>
public class GameStarter {
    private GameLogger logger;
    private RoundManager roundManager;
    public bool GameStarted { get; private set; }

    public GameStarter(GameLogger logger, RoundManager roundManager) {
        this.logger = logger;
        this.roundManager = roundManager;
        GameStarted = false;
    }

    public void StartGame() {
        DatabaseReferences.GetRoomReference().Child(LocalStorage.GAME_STARTED).SetValueAsync(true);
        logger.TranslatedLog("game-begun");
        GameStarted = true;
        roundManager.CallNextPlayer();
    }
}
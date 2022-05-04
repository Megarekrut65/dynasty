public class GameStarter {
    private GameLogger logger;
    private RoundManager roundManager;
    public bool GameOver { get; set; } = false;
    public bool GameStarted { get; private set; }

    public GameStarter(GameLogger logger, RoundManager roundManager) {
        this.logger = logger;
        this.roundManager = roundManager;
        GameStarted = false;
    }
    
    public void StartGame() {
        logger.TranslatedLog("game-begun");
        GameStarted = true;
        roundManager.CallNextPlayer();
    }
}
public class GameController {
    private GameLogger logger;
    private RoundManager roundManager;
    public bool GameOver { get; set; } = false;
    
    public GameController(GameLogger logger, RoundManager roundManager) {
        this.logger = logger;
        this.roundManager = roundManager;
    }
    
    public void StartGame() {
        logger.TranslatedLog("game-begun");
        roundManager.CallNextPlayer();
    }
}
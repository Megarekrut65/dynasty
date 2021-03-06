/// <summary>
/// Implementation of GameController for offline mode
/// </summary>
public class OfflineGameController : GameController {
    public OfflineGameController(GameDependencies gameDependencies, CardDependencies cardDependencies,
        CardTaker cardTaker) :
        base(gameDependencies, cardDependencies, cardTaker) {
        LoadPlayers();
        StartGame();
        GameCloser.GameOverEvent += Leave;
    }
    public override void Leave() {
        GameCloser.GameOverEvent -= Leave;
        resultCreator.MakeResult();
        OpenScene("GameOver");
    }
    private void LoadPlayers() {
        string botType = (LocalStorage.GetValue(LocalStorage.DIFFICULTY_BOTS) == EntityControllerFactory.MEDIUM
            ? "Medium"
            : "Easy") + "Bot";
        for (int i = gameDependencies.playerManager.GetPlayersCount();
             i < gameDependencies.playerManager.GetEntityCount(); i++) {
            var player = gameDependencies.playerManager.AddController(
                botType + i, gameDependencies, cardDependencies.Table, cardTaker.TakeCardFromDesk);
            cardDependencies.Table.AddPlayer(player);
        }
    }
}
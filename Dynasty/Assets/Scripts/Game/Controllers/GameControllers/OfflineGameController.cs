public class OfflineGameController:GameController {
    public OfflineGameController(GameDependencies gameDependencies, CardDependencies cardDependencies, 
        CardTaker cardTaker) : 
        base(gameDependencies, cardDependencies, cardTaker) {
        LoadPlayers();
        StartGame();
    }
    public override void Leave() {
        OpenScene(null);
    }
    private void LoadPlayers() {
        string botType = (LocalStorage.GetValue(LocalStorage.DIFFICULTY_BOTS) == EntityControllerFactory.MEDIUM
            ? "Medium"
            : "Easy") + "Bot";
        for (int i = gameDependencies.playerManager.GetPlayersCount(); i < gameDependencies.playerManager.GetEntityCount(); i++) {
            var player = gameDependencies.playerManager.AddController(
                botType + i, gameDependencies, cardDependencies.Table, cardTaker.TakeCardFromDesk);
            cardDependencies.Table.AddPlayer(player);
        }
    }
}
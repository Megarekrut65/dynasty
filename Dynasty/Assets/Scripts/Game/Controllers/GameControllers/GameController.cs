using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public abstract class GameController {
    protected GameDependencies gameDependencies;
    protected CardDependencies cardDependencies;
    protected CardTaker cardTaker;
    protected ResultCreator resultCreator;
    
    protected GameController(GameDependencies gameDependencies, CardDependencies cardDependencies, CardTaker cardTaker) {
        this.gameDependencies = gameDependencies;
        this.cardDependencies = cardDependencies;
        this.cardTaker = cardTaker;
        resultCreator = new ResultCreator(gameDependencies.playerManager, gameDependencies.gameCloser);
    }
    protected void StartGame() {
        cardDependencies.AddStartCards();
        gameDependencies.gameStarter.StartGame();
    }
    public abstract void Leave();
    protected void OpenScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
public class GameCloser {
    public bool GameOver { get; set; } = false;
    public bool GameClosed { get; private set; } = false;
    public delegate void TheGameOver();
    public static event TheGameOver theGameOver;
    public void DoGameOver() {
        GameClosed = true;
        theGameOver?.Invoke();
    }
}
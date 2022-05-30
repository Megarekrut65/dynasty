/// <summary>
/// Class that send message after game finished
/// </summary>
public class GameCloser {
    public bool GameOver { get; set; } = false;
    public bool GameClosed { get; private set; } = false;
    public delegate void TheGameOver();
    public static event TheGameOver GameOverEvent;
    public void DoGameOver() {
        GameClosed = true;
        GameOverEvent?.Invoke();
    }
}
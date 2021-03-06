using System.Collections.Generic;

/// <summary>
/// Class that control players rounds
/// </summary>
public class RoundManager {
    public delegate void NextRound();
    public event NextRound Next;

    private int index = 0;
    private int rounds = 0;
    public bool Pause { get; set; } = false;
    private Player current = null;
    private List<Player> players;

    public RoundManager(List<Player> players) {
        this.players = players;
    }
    public void AddMoreRounds(int moreRounds) {
        rounds += moreRounds;
    }
    public Player GetTheNextPlayer() {
        int i;
        if (rounds > 0) {
            i = index - 1;
            if (i < 0) i = players.Count - 1;
            rounds--;
        } else {
            i = index++;
            rounds = 0;
        }

        current = players[i];
        if (index >= players.Count) index = 0;

        return current;
    }
    public Player WhoIsNextPlayer() {
        int i;
        if (rounds > 0) {
            i = index - 1;
            if (i < 0) i = players.Count - 1;
        } else i = index;

        return players[i];
    }
    public Player WhoIsNow() {
        return current;
    }
    public void CallNextPlayer() {
        Pause = false;
        Next?.Invoke();
    }
}
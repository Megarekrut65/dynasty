using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void NextRound();
    public event NextRound Next;
    private bool gameOver;
    public bool GameOver
    {
        get
        {
            return gameOver;
        }
        set
        {
            gameOver = value;
        }
    }
    private int playerCount = 1;
    public int PlayerCount
    {
        get
        {
            return playerCount;
        }
    }
    private int botCount = 5;
    public int BotCount
    {
        get
        {
            return botCount;
        }
    }
    private List<Player> players;
    public List<Player> Players
    {
        get
        {
            return players;
        }
    }
    private string[] nicknames = new string[6];
    private int index = 0;
    private int cardCount = 0;
    private bool pause = false;
    public bool Pause
    {
        get
        {
            return pause;
        }
        set
        {
            pause = value;
        }
    }

    void Start()
    {

    }
    public void CreatePlayers()
    {
        players = new List<Player>();
        for (int i = 0; i < nicknames.Length; i++)
        {
            nicknames[i] = "Player" + UnityEngine.Random.Range(0, 1000);
        }
        nicknames[0] = "You";
        for (int i = 0; i < playerCount + botCount; i++)
        {
            players.Add(new Player(nicknames[i]));
        }
    }
    public Player NextPlayer()
    {
        int i;
        if (cardCount > 0)
        {
            i = index - 1;
            if (i < 0) i = playerCount + botCount - 1;
            cardCount--;
        }
        else
        {
            i = index++;
            cardCount = 0;
        }
        Player player = players[i];
        if (index >= playerCount + botCount) index = 0;

        return player;
    }
    public Player GetNextPlayer()
    {
        int i;
        if (cardCount > 0)
        {
            i = index - 1;
            if (i < 0) i = playerCount + botCount - 1;
        }
        else i = index;

        return players[i];
    }
    public void AddCount(int add)
    {
        cardCount += add;
    }
    public void CallNext()
    {
        pause = false;
        Next?.Invoke();
    }
}
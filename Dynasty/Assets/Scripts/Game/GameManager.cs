using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public delegate void NextRound();
    public event NextRound Next;
    private bool gameOver;
    public bool GameOver{
        get{
            return gameOver;
        }
        set{
            gameOver = value;
        }
    }
    private int playerCount = 1;
    private int botCount = 5;
    public int BotCount{
        get{
            return botCount;
        }
    }
    private List<Player> players;
    public List<Player> Players{
        get{
            return players;
        }
    }
    private string[] nicknames = new string[6];
    private int index = 0;

    void Start(){
        
    }
    public void CreatePlayers(){
        players = new List<Player>();
        for(int i = 0; i < nicknames.Length; i++){
            nicknames[i] = "Player" + UnityEngine.Random.Range(0, 1000);
        }
        nicknames[0] = "You";
        for(int i = 0; i < playerCount + botCount; i++){
            players.Add(new Player(nicknames[i]));
        }
    }
    public Player NextPlayer(){
        Player player = players[index++];
        if(index >= playerCount + botCount){
            index = 0;
        }
        return player;
    }
    public Player GetNextPlayer(){
        return players[index];
    }
    public void CallNext(){
        Next?.Invoke();
    }
}
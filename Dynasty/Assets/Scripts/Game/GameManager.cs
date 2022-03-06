using UnityEngine;

public class GameManager : MonoBehaviour {
    private bool gameOver;
    public bool GameOver{
        get{
            return gameOver;
        }
        set{
            gameOver = value;
        }
    }
}
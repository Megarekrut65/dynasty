using UnityEngine;
using Random = System.Random;

public class ChangeMode : MonoBehaviour {
    [SerializeField]
    private GameMode gameMode;
    public void Change() {
        PlayerPrefs.SetString(LocalStorage.GAME_MODE, gameMode.ToString());
        PlayerPrefs.SetInt(LocalStorage.DESK_SEED, new Random().Next());
    }
}
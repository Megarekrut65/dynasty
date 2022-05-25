using UnityEngine;
using UnityEngine.UI;

public class GameOverLoader : MonoBehaviour {
    [SerializeField]
    private Text body;

    private void Start() {
        body.text = LocalStorage.GetValue(LocalStorage.GAME_RESULT);
    }
}
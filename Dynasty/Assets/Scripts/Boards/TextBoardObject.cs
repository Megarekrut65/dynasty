using UnityEngine;
using UnityEngine.UI;

public class TextBoardObject : MonoBehaviour {
    [SerializeField]
    private Text text;

    public void HideBoard() {
        gameObject.SetActive(false);
    }
    public void SetText(string str) {
        text.text = str;
    }
}
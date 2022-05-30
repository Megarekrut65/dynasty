using UnityEngine;
using UnityEngine.UI;

public class FullScreenCardMaker:MonoBehaviour {
    [SerializeField]
    private CardLoader cardLoader;
    [SerializeField]
    private FullScreenButton fullScreenButton;
    [SerializeField]
    private Outline outline;
    [SerializeField]
    private GameObject background;

    public void Hide() {
        background.SetActive(false);
    }
    public void Make(IClick click, string key, Color color) {
        cardLoader.Key = key;
        cardLoader.LoadData();
        fullScreenButton.Click = click;
        if (color.a > 0f) color.a = 0.5f;
        outline.effectColor = color;
        background.SetActive(true);
    }
}
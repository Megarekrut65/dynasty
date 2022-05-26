using UnityEngine;
using UnityEngine.UI;

public class MessageBox : GameLogger {
    [SerializeField]
    private Scrollbar verticalScroll;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Text content;

    public void Change(bool isOpen) {
        var rect = gameObject.GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(rect.offsetMax.x, isOpen ? 150f : 0f);
        verticalScroll.interactable = isOpen;
        scrollRect.vertical = isOpen;
        scrollRect.normalizedPosition = new Vector2(0f, 0f);
    }
    public override void Log(string text) {
        if (content.text.Length > 3000) content.text = content.text.Substring(2900);
        content.text += text + "\r\n";
        scrollRect.normalizedPosition = new Vector2(0f, 0f);
    }
}
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
		rect.offsetMax = new Vector2(rect.offsetMax.x, isOpen ? Screen.height / 3 : 0f);
		verticalScroll.interactable = isOpen;
		scrollRect.vertical = isOpen;
		scrollRect.normalizedPosition = new Vector2(0, 0);
	}
	public override void Log(string text) {
		content.text += text + "\r\n";
		scrollRect.normalizedPosition = new Vector2(0, 0);
	}
}
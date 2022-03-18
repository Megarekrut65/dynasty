using UnityEngine;
using UnityEngine.UI;
public class ScrollManager {
	private ScrollRect scrollRect;
	private RectTransform content;
	private GameObject contentObject;
	public ScrollManager(ScrollRect scrollRect, GameObject contentObject) {
		this.scrollRect = scrollRect;
		this.content = contentObject.GetComponent<RectTransform>();
		this.contentObject = contentObject;
	}
	public void ScrollTo(RectTransform target) {
		Canvas.ForceUpdateCanvases();
		content.anchoredPosition =
				(Vector2)scrollRect.transform.InverseTransformPoint(content.position)
				- (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
	}
	public void AddToScroll(GameObject obj) {
		obj.transform.SetParent(contentObject.transform, false);
	}
}
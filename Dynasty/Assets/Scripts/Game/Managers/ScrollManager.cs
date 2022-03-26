using UnityEngine;
using UnityEngine.UI;

public class ScrollManager {
	private ScrollRect scrollRect;
	private RectTransform content;
	private GameObject contentObject;
	private GameObject view;

	public ScrollManager(ScrollRect scrollRect, GameObject contentObject, GameObject view) {
		this.scrollRect = scrollRect;
		this.content = contentObject.GetComponent<RectTransform>();
		this.contentObject = contentObject;
		this.view = view;
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
	public void ViewSetActive(bool isActive) {
		view.SetActive(isActive);
	}
}
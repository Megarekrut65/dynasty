using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HideShowButton : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	[SerializeField]
	private List<GameObject> toHide;
	[SerializeField]
	private List<GameObject> toShow;

	public void OnPointerDown(PointerEventData eventData) {

	}
	public void OnPointerUp(PointerEventData eventData) {
		toHide.ForEach(obj => obj.SetActive(false));
		toShow.ForEach(obj => obj.SetActive(true));
	}
}
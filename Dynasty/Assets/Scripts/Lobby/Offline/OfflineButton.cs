using UnityEngine;
using UnityEngine.EventSystems;

public class OfflineButton : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {

	public void OnPointerDown(PointerEventData eventData) {
		PlayerPrefs.SetString("game-mode", "offline");
	}
	public void OnPointerUp(PointerEventData eventData) {

	}
}
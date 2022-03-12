using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class LocalizationButton : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	[SerializeField]
	private string language = "";
	public void OnPointerDown(PointerEventData eventData) {
		LocalizationManager.instance.ChangeLanguage(language);
	}
	public void OnPointerUp(PointerEventData eventData) {

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	private ButtonEffect buttonEffect;
	[SerializeField]
	private GameObject soundClick;
	private void Start() {
		buttonEffect = new ButtonEffect(transform, soundClick);
	}
	public void OnPointerDown(PointerEventData eventData) {
		buttonEffect.Down();
	}
	public void OnPointerUp(PointerEventData eventData) {
		buttonEffect.Up();
	}
}

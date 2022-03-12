using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClick : MonoBehaviour,
	IPointerDownHandler, IPointerUpHandler {
	private Func<bool> click;
	public Func<bool> Click {
		set {
			click = value;
		}
	}
	private bool canClick;
	public bool CanClick {
		set {
			canClick = value;
		}
	}
	private ButtonEffect buttonEffect;
	void Start() {
		buttonEffect = new ButtonEffect(transform);
	}
	public void OnPointerDown(PointerEventData eventData) {
		if (canClick || eventData == null) buttonEffect.Down();
	}
	public void OnPointerUp(PointerEventData eventData) {
		if (canClick || eventData == null) {
			buttonEffect.Up();
			click();
		}
	}
}
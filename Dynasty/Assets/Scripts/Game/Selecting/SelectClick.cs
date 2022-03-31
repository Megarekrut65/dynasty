using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectClick : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	private int id;
	public int Id {
		set => id = value;
	}
	private Action<int> select;
	public Action<int> Select {
		set => select = value;
	}
	private bool canClick;
	public bool CanClick {
		set => canClick = value;
	}
	private ButtonEffect buttonEffect;
	private void Start() {
		buttonEffect = new ButtonEffect(transform);
	}
	public void OnPointerDown(PointerEventData eventData) {
		//if (canClick || eventData == null) buttonEffect.Down();
	}
	public void OnPointerUp(PointerEventData eventData) {
		if (canClick || eventData == null) {
			buttonEffect.Up();
			select(id);
		}
	}
}
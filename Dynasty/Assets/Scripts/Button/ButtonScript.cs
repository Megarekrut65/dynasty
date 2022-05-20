using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	[SerializeField]
	private UnityEvent downEvent;
	[SerializeField]
	private UnityEvent upEvent;
	private ButtonEffect buttonEffect;

	private void Start() {
		buttonEffect = new ButtonEffect(transform,downEvent, upEvent, true);
	}
	public void OnPointerDown(PointerEventData eventData) {
		buttonEffect.Down();
	}
	public void OnPointerUp(PointerEventData eventData) {
		buttonEffect.Up();
	}
}

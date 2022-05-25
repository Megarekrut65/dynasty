using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private bool needSound = true;
    [SerializeField]
    private int soundIndex = 0;
    [SerializeField]
    private UnityEvent downEvent;
    [SerializeField]
    private UnityEvent upEvent;
    private ButtonEffect buttonEffect;

    private void Start() {
        buttonEffect = new ButtonEffect(transform, downEvent, upEvent, needSound, soundIndex);
    }
    public void OnPointerDown(PointerEventData eventData) {
        buttonEffect.Down();
    }
    public void OnPointerUp(PointerEventData eventData) {
        buttonEffect.Up();
    }
}
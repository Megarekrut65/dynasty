using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CardEffect = System.Func<bool>;

public class CardClick : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler {
    private ButtonEffect buttonEffect;
    public Action<PointerEventData> Up { set; private get; }
    public Action<PointerEventData> Down { set; private get; }

    private void Start() {
        buttonEffect = new ButtonEffect(transform, null, null, true, 3);
    }
    public void OnPointerDown(PointerEventData eventData) {
        buttonEffect.Down();
        Down(eventData);
    }
    public void OnPointerUp(PointerEventData eventData) {
        buttonEffect.Up();
        Up(eventData);
    }
}
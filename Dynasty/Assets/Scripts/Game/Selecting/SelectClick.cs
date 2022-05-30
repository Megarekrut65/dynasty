using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectClick : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler {
    public int Id { get; set; }
    public Action<PointerEventData> Down { set; private get; }
    public Action<PointerEventData> Up { set; private get; }
    
    public void OnPointerDown(PointerEventData eventData) {
        Down(eventData);
    }
    public void OnPointerUp(PointerEventData eventData) {
        Up(eventData);
    }
}
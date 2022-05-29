using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardFullScreenClick : MonoBehaviour ,
    IPointerDownHandler, IPointerUpHandler {
    public IClick Click { set; private get; }
    public Action HideBackground { set; private get; }
    
    public void OnPointerDown(PointerEventData eventData) {
        Click.Down(eventData);
    }
    public void OnPointerUp(PointerEventData eventData) {
        if(Click.Up(eventData))
            HideBackground();
    }
}
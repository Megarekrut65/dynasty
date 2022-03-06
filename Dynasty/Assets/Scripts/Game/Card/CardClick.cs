using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClick : MonoBehaviour, 
    IPointerDownHandler, IPointerUpHandler {
    private Func<bool> click;
    public Func<bool> Click{
        set{
            click = value;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {   

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        click();
    }
}
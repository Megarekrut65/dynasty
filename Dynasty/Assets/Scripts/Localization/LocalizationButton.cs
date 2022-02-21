using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class LocalizationButton : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private LocalizationManager manager;
    [SerializeField]
    private string language = "";
    public void OnPointerDown(PointerEventData eventData)
    {   
        manager.ChangeLanguage(language);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}

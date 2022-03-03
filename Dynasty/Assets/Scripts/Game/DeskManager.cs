using UnityEngine;
using UnityEngine.EventSystems;
public class DeskManager : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private TableManager manager;

    public void OnPointerDown(PointerEventData eventData)
    {   
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        manager.TakeCardFromDesk();
    }
}
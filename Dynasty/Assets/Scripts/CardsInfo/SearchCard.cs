using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SearchCard : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private GeneratorManager manager;

    public void OnPointerDown(PointerEventData eventData)
    {   
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //manager.SelectName(input.text);
    }
}
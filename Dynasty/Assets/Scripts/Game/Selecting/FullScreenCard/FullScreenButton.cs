using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class for big card in scene
/// </summary>
public class FullScreenButton : MonoBehaviour ,
    IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private GameObject background;
    public IClick Click { set; private get; }
    
    public void OnPointerDown(PointerEventData eventData) {
        Click.Down(eventData);
    }
    public void OnPointerUp(PointerEventData eventData) {
        if(Click.Up(eventData)) background.SetActive(false);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SliderZoom : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private CameraMove cameraMove;
    public void Change(){
        mainCamera.orthographicSize = slider.value;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        cameraMove.Stop = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {   
        cameraMove.Stop = true;
    }
    void Start(){
        slider.value = mainCamera.orthographicSize;
    }
}
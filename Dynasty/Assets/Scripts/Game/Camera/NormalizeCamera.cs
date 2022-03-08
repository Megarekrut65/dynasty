using UnityEngine;
using UnityEngine.EventSystems;
public class NormalizeCamera : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private CameraMove controller;
    public void OnPointerUp(PointerEventData eventData)
    {
        controller.Stop = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {   
        controller.Stop = true;
        cameraTransform.position = new Vector3(0f, 0f, cameraTransform.position.z);
    }
}
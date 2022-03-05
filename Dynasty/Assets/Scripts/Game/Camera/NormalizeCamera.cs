using UnityEngine;
using UnityEngine.EventSystems;
public class NormalizeCamera : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private CameraMove cameraMove;
    public void OnPointerUp(PointerEventData eventData)
    {
        cameraMove.Stop = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {   
        cameraMove.Stop = true;
        cameraTransform.position = new Vector3(0f, 0f, cameraTransform.position.z);
    }
}
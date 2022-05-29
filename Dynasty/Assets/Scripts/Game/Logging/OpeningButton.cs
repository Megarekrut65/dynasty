using UnityEngine;
using UnityEngine.EventSystems;

public class OpeningButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private CameraMove cameraMove;
    [SerializeField]
    private MessageBox messageBox;
    private bool isOpen = false;

    public void OnPointerDown(PointerEventData eventData) {
        Change(!isOpen);
    }
    public void OnPointerUp(PointerEventData eventData) {
    }
    public void Change(bool open) {
        if(isOpen == open) return;
        isOpen = open;
        cameraMove.Stop = isOpen;
        gameObject.transform.Rotate(0f, 0f, isOpen ? -180f : 180f, Space.Self);
        messageBox.Change(isOpen);
    }
}
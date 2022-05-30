using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Stops camera moving and go to center of scene
/// </summary>
public class NormalizeCamera : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private CameraMove controller;
    [SerializeField]
    private OpeningButton openingButton;

    public void OnPointerUp(PointerEventData eventData) {
        controller.Stop = false;
        openingButton.Change(false);
    }
    public void OnPointerDown(PointerEventData eventData) {
        controller.Stop = true;
        cameraTransform.position = new Vector3(0f, 0f, cameraTransform.position.z);
    }
}
using UnityEngine;
using System.Collections;
public class CameraZoom : MonoBehaviour {
    [SerializeField]
    private Camera cam;
    private Touch oldTouch1;
    private Touch oldTouch2;
    void Update()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }     
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);
        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            return;
        }
        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        float offset = newDistance - oldDistance;
        float scaleFactor = offset / 100f;
        cam.orthographicSize *= scaleFactor;
        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
    }
}
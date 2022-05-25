using UnityEngine;

public class CameraMove : MonoBehaviour {
    [Header("Camera")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform cameraTransform;
    [Header("Settings")]
    [SerializeField]
    private Vector2 max;
    [SerializeField]
    private Vector2 min;
    [SerializeField]
    private float speed;
    private Vector2 start;
    private Vector3 targetPosition;
    private bool stop = false;
    public bool Stop {
        set => stop = value;
    }

    private void Start() {
        targetPosition = cameraTransform.position;
    }
    private void Update() {
        if (Input.GetMouseButtonDown(0)) start = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        else if (Input.GetMouseButton(0)) {
            Vector2 position = (Vector2) mainCamera.ScreenToWorldPoint(Input.mousePosition) - start;
            targetPosition = new Vector3(
                Mathf.Clamp(cameraTransform.position.x - position.x, min.x, max.x),
                Mathf.Clamp(cameraTransform.position.y - position.y, min.y, max.y),
                cameraTransform.position.z);
        }

        if (stop) targetPosition = cameraTransform.position;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, speed * Time.deltaTime);
    }
}
using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Camera")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform cameraTransform;
    [Header("Move Settings")]
    [SerializeField]
    private Vector2 moveMax;
    [SerializeField]
    private Vector2 moveMin;
    [SerializeField]
    private float speed;
    private Vector2 start;
    private Vector3 targetPosition;

    private float targetSize;
	private float touchesPrevPosDifference; 
    private float touchesCurPosDifference;
	[Header("Zoom Settings")]
	[SerializeField]
	private int zoomMin = 150;
	[SerializeField]
    private int zoomMax = 500;
	[SerializeField]
	private float zoomModifierSpeed = 200f;
    private bool stop = false;
    public bool Stop{
        set{
            stop = value;
        }
    }
    void Start(){
        targetPosition = cameraTransform.position;
        targetSize = mainCamera.orthographicSize;
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer){
            zoomModifierSpeed /= 40f;
        }
    }
    void Update() {
        if(Input.GetMouseButtonDown(0)){
            start = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        } 
        if(!Zoom() && Input.GetMouseButton(0)){
            Move();
        }
        ZoomUpdate();
        MoveUpdate();
    }
    private void MoveUpdate(){
        if(stop) targetPosition = cameraTransform.position;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, speed*Time.deltaTime);
    }
    private void Move(){
        Vector2 position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - start;
        targetPosition = new Vector3(
            Mathf.Clamp(cameraTransform.position.x - position.x, moveMin.x, moveMax.x),
            Mathf.Clamp(cameraTransform.position.y - position.y, moveMin.y, moveMax.y), 
            cameraTransform.position.z);
    }
    private void ZoomUpdate(){
        if(stop) targetSize = mainCamera.orthographicSize;
        else targetSize = Mathf.Clamp (targetSize, zoomMin, zoomMax);
		mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime);
    }
    private bool Zoom(){
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer){
			if (Input.touchCount == 2) {
				MobileZoom();
                return true;
			}
        }
		else DesktopZoom();
        return false;
    }
    private void DesktopZoom(){
        targetSize -= Input.GetAxis("Mouse ScrollWheel") * zoomModifierSpeed;
    }
    private void MobileZoom(){
		float zoomModifier = CountZoom();
		if (touchesPrevPosDifference > touchesCurPosDifference){
			targetSize += zoomModifier;
		}
		if (touchesPrevPosDifference < touchesCurPosDifference){
			targetSize -= zoomModifier;
		}
    }
	private float CountZoom(){
		Touch firstTouch = Input.GetTouch(0);
		Touch secondTouch = Input.GetTouch(1);
		Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
		Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;
		touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
		touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;
		return (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;
	}
}
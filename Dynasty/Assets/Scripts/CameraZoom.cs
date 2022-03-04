using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class CameraZoom : MonoBehaviour {
    private int min = 150;
    private int max = 500;
    [SerializeField]
    private Camera mainCamera;

	private float touchesPrevPosDifference; 
    private float touchesCurPosDifference;
    private float zoomModifier;

	private Vector2 firstTouchPrevPos;
    private Vector2 secondTouchPrevPos;

	[SerializeField]
	private float zoomModifierSpeed = 0.1f;
	void Update () {
		
		if (Input.touchCount == 2) {
			Touch firstTouch = Input.GetTouch (0);
			Touch secondTouch = Input.GetTouch (1);

			firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
			secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

			touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
			touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

			zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

			if (touchesPrevPosDifference > touchesCurPosDifference)
				mainCamera.orthographicSize += zoomModifier;
			if (touchesPrevPosDifference < touchesCurPosDifference)
				mainCamera.orthographicSize -= zoomModifier;
			
		}

		mainCamera.orthographicSize = Mathf.Clamp (mainCamera.orthographicSize, min, max);
	}
}
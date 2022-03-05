using System;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class CameraZoom : MonoBehaviour {

    [SerializeField]
    private Camera mainCamera;
	private float targetSize;
	private float touchesPrevPosDifference; 
    private float touchesCurPosDifference;
	[Header("Settings")]
	[SerializeField]
	private int min = 150;
	[SerializeField]
    private int max = 500;
	[SerializeField]
	private float zoomModifierSpeed = 0.1f;
	
	void Start(){
		targetSize = mainCamera.orthographicSize;
	}
	void Update() {
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer){
			if (Input.touchCount == 2) {
				zoomModifierSpeed /= 40f;
				float zoomModifier = CountZoom();
				if (touchesPrevPosDifference > touchesCurPosDifference){
					targetSize += zoomModifier;
				}
				if (touchesPrevPosDifference < touchesCurPosDifference){
					targetSize -= zoomModifier;
				}
			}
		}
		else{
			targetSize -= Input.GetAxis("Mouse ScrollWheel") * zoomModifierSpeed;
		}
		targetSize = Mathf.Clamp (targetSize, min, max);
		mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime);
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
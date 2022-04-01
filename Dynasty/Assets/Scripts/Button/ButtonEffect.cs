using UnityEngine;
using UnityEngine.Events;

public class ButtonEffect {
	private Transform transform;
	private Vector3 scale;
	private GameObject soundClick;
	private UnityEvent downEvent;
	private UnityEvent upEvent;
	
	public ButtonEffect(Transform transform, UnityEvent downEvent = null, UnityEvent upEvent = null, GameObject soundClick = null) {
		this.transform = transform;
		this.downEvent = downEvent;
		this.upEvent = upEvent;
		this.scale = transform.localScale;
		this.soundClick = soundClick;
	}
	public void Down() {
		transform.localScale = 1.1f * scale;
		downEvent?.Invoke();
		if (soundClick != null) soundClick.GetComponent<AudioSource>().Play();
	}
	public void Up() {
		transform.localScale = scale;
		upEvent?.Invoke();
	}
}
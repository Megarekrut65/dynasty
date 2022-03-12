using UnityEngine;

public class ButtonEffect {
	private Transform transform;
	private Vector3 scale;
	private GameObject soundClick;
	public ButtonEffect(Transform transform, GameObject soundClick = null) {
		this.transform = transform;
		this.scale = transform.localScale;
		this.soundClick = soundClick;
	}
	public void Down() {
		transform.localScale = 1.1f * scale;
		if (soundClick != null) soundClick.GetComponent<AudioSource>().Play();
	}
	public void Up() {
		transform.localScale = scale;
	}
}
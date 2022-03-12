using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	private MovingData data = null;
	public MovingData Data {
		set {
			data = value;
			transform.position = data.startPostion;
		}
	}
	void Update() {
		if (data != null) {
			transform.position = Vector3.Lerp(data.startPostion, data.endPosition, data.speed * Time.deltaTime);
			if ((transform.position - data.endPosition).magnitude < 0.5f) {
				data.endFunc();
			}
		}
	}
}


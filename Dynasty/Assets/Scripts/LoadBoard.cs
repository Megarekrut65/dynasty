using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

public class LoadBoard {
	private GameObject board = null;

	public LoadBoard(GameObject blackBoard, GameObject _canvas) {
		if (blackBoard != null && _canvas != null) {
			board = MonoBehaviour.Instantiate(blackBoard, new Vector3(0, 0, 0), Quaternion.identity);
			board.GetComponent<RectTransform>().sizeDelta = _canvas.GetComponent<RectTransform>().sizeDelta;
			board.transform.SetParent(_canvas.transform, false);
		}
	}
	public void Destroy() {
		if (board != null) {
			MonoBehaviour.Destroy(board);
		}
	}
	public void SetActive(bool value) {
		board.SetActive(value);
	}
}
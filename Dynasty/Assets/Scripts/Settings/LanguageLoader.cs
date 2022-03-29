using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanguageLoader : MonoBehaviour {
	[SerializeField]
	private GameObject blackBoard;
	[SerializeField]
	private GameObject mainCanvas;
	private LoadBoard loadBoard;

	private void Start() {
		loadBoard = new LoadBoard(blackBoard, mainCanvas);
	}
	public void SetActive(bool value) {
		loadBoard.SetActive(value);
	}
}
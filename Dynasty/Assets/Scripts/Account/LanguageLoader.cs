using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanguageLoader : MonoBehaviour {
	[SerializeField]
	private GameObject blackBoard;
	private GameObject _canvas;
	void Start() {
		_canvas = GameObject.Find("Canvas");
		StartCoroutine("LoadData");
	}

	IEnumerator LoadData() {
		LoadBoard loadBoard = new LoadBoard(blackBoard, _canvas);
		while (!LocalizationManager.Instance.Ready) {
			yield return new WaitForSeconds(0.005f);
		}
		loadBoard.Destroy();
	}
}
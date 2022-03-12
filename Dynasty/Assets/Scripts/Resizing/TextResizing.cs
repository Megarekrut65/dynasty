using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResizing : MonoBehaviour {
	private static ResizingData data = new ResizingData(1000);
	[SerializeField]
	private Text text;

	void Awake() {
		data.OnChanged += ChangeText;
	}
	void OnDestroy() {
		data.OnChanged -= ChangeText;
	}
	void Start() {
		StartCoroutine(MakeTextSameSize());
	}
	IEnumerator MakeTextSameSize() {
		yield return null;

		int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
		data.MinFontSize = size;
		ChangeText();
	}
	void ChangeText() {
		text.resizeTextMaxSize = data.MinFontSize;
	}
}
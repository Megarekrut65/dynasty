using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResizing : MonoBehaviour {
	private static ResizingData data = new ResizingData(1000);
	[SerializeField]
	private Text text;

	private void Awake() {
		data.OnChanged += ChangeText;
	}
	private void OnDestroy() {
		data.OnChanged -= ChangeText;
	}
	private void Start() {
		StartCoroutine(MakeTextSameSize());
	}
	private IEnumerator MakeTextSameSize() {
		yield return null;

		int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
		data.MinFontSize = size;
		ChangeText();
	}
	private void ChangeText() {
		text.resizeTextMaxSize = data.MinFontSize;
	}
}
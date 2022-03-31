using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextResizing : MonoBehaviour {
	private static ResizingData _data = new ResizingData(1000);
	[SerializeField]
	private Text text;

	private void Awake() {
		_data.OnChanged += ChangeText;
	}
	private void OnDestroy() {
		_data.OnChanged -= ChangeText;
	}
	private void Start() {
		StartCoroutine(MakeTextSameSize());
	}
	private IEnumerator MakeTextSameSize() {
		yield return null;

		int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
		_data.MinFontSize = size;
		ChangeText();
	}
	private void ChangeText() {
		text.resizeTextMaxSize = _data.MinFontSize;
	}
}
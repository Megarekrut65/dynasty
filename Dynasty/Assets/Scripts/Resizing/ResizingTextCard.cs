using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResizingTextCard : MonoBehaviour {
	[SerializeField]
	private Text text;
	[SerializeField]
	private Text title;
	private ResizingData data;
	private bool addedToChange = false;

	public void Resize(ResizingData data) {
		this.data = data;
		addedToChange = true;
		data.OnChanged += ChangeText;
		int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
		data.MinFontSize = size;
		ChangeText();
	}
	private void OnDestroy() {
		if (addedToChange) data.OnChanged -= ChangeText;
		addedToChange = false;
	}
	public void BestFit(bool active) {
		text.resizeTextForBestFit = active;
	}
	void ChangeText() {
		text.resizeTextMaxSize = data.MinFontSize;
	}
}
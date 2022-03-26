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

	public void Resize(ResizingData data) {
		this.data = data;
		data.OnChanged += ChangeText;
		int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
		data.MinFontSize = size;
		ChangeText();
	}
	public void BestFit(bool active) {
		text.resizeTextForBestFit = active;
	}
	void ChangeText() {
		int delta = data.MinFontSize - text.cachedTextGenerator.fontSizeUsedForBestFit;
		if (Math.Abs(delta) > 5) {
			text.resizeTextMaxSize = Math.Min(text.cachedTextGenerator.fontSizeUsedForBestFit,
		   		title.cachedTextGenerator.fontSizeUsedForBestFit);
		} else text.resizeTextMaxSize = data.MinFontSize;
	}
}
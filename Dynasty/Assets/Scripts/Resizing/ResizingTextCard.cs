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
	void ChangeText() {
		text.resizeTextMaxSize = Math.Min(data.MinFontSize,
		   title.cachedTextGenerator.fontSizeUsedForBestFit);
	}
} 
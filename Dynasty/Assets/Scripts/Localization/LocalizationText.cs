using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocalizationText : MonoBehaviour {
	[SerializeField]
	private string key;
	private Text text;

	private void Awake() {
		if (text == null) {
			text = GetComponent<Text>();
		}
		LocalizationManager.Instance.OnLanguageChanged += UpdateText;
	}
	private void Start() {
		UpdateText();
	}

	private void OnDestroy() {
		LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
	}
	virtual protected void UpdateText() {
		if (gameObject == null) return;
		if (text == null) {
			text = GetComponent<Text>();
		}
		text.text = LocalizationManager.Instance.GetWord(key);
	}
}

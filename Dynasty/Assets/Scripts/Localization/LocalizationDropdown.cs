using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationDropdown : MonoBehaviour {
	[SerializeField]
	private Text label;
	[SerializeField]
	private Dropdown dropdown;

	private void Start() {
		var options = dropdown.options;
		label.text = LocalizationManager.Instance.GetWord(options[0].text);
		foreach (var data in options) {
			data.text = LocalizationManager.Instance.GetWord(data.text);
		}
		dropdown.value = 0;
	}
}

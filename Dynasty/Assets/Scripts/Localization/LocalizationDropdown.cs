using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationDropdown : MonoBehaviour {
	[SerializeField]
	private Text label;
	[SerializeField]
	private Dropdown dropdown;
	private List<string> keys = new List<string>();
	
	private void Start() {
		var options = dropdown.options;
		foreach (var data in options) {
			keys.Add(data.text);
			data.text = LocalizationManager.Instance.GetWord(data.text);
		}
		dropdown.value = 0;
	}
	public void ChangeValue(int value) {
		label.text = LocalizationManager.Instance.GetWord(
			keys[value]);
	}
}

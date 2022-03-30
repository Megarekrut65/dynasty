using UnityEngine;
using UnityEngine.UI;

public class DifficultyBots : MonoBehaviour {
	[SerializeField]
	private Dropdown dropdown;
	private string key = "difficulty-bots";

	public void Change(int value) {
		PlayerPrefs.SetString(key, dropdown.options[value].text.ToLower());
	}
}
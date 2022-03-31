using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyBots : MonoBehaviour {
	[SerializeField]
	private Dropdown dropdown;
	private List<string> keys = new List<string>();

	private void Start() {
		dropdown.options.ForEach(op=>keys.Add(op.text));
	}
	public void Change(int value) {
		PlayerPrefs.SetString(PrefabsKeys.DIFFICULTY_BOTS, keys[value]);
	}
}
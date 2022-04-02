using System;
using UnityEngine;
using UnityEngine.UI;

public class EnableBots : MonoBehaviour {
	[SerializeField]
	private GameObject hide;
	[SerializeField]
	private Toggle toggle;

	private void Start() {
		toggle.onValueChanged.AddListener(Change);
		toggle.isOn = Convert.ToBoolean(PrefabsKeys.GetValue(PrefabsKeys.ENABLE_BOTS, false.ToString()));
	}
	private void Change(bool value) {
		hide.SetActive(!value);
		PlayerPrefs.SetString(PrefabsKeys.ENABLE_BOTS, value.ToString());
	}
}
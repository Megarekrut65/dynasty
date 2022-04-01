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
		toggle.isOn = PlayerPrefs.HasKey(PrefabsKeys.ENABLE_BOTS) &&
		              Convert.ToBoolean(PlayerPrefs.GetString(PrefabsKeys.ENABLE_BOTS));
	}
	private void Change(bool value) {
		hide.SetActive(!value);
		PlayerPrefs.SetString(PrefabsKeys.ENABLE_BOTS, value.ToString());
	}
}
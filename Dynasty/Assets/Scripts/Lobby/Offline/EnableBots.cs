using System;
using UnityEngine;
using UnityEngine.UI;

public class EnableBots : MonoBehaviour {
	[SerializeField]
	private GameObject hide;
	[SerializeField]
	private Toggle toggle;

	private void Start() {
		if (PlayerPrefs.HasKey(PrefabsKeys.ENABLE_BOTS)) {
			toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetString(PrefabsKeys.ENABLE_BOTS));
		}
	}
	public void Change(bool value) {
		hide.SetActive(!value);
		PlayerPrefs.SetString(PrefabsKeys.ENABLE_BOTS, value.ToString());
	}
}
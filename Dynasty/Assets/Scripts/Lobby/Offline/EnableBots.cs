using System;
using UnityEngine;
using UnityEngine.UI;

public class EnableBots : MonoBehaviour {
	[SerializeField]
	private GameObject hide;
	[SerializeField]
	private Toggle toggle;
	private string key = "enable-bots";

	private void Start() {
		if (PlayerPrefs.HasKey(key)) {
			toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetString(key));
		}
	}
	public void Change(bool value) {
		hide.SetActive(!value);
		PlayerPrefs.SetString(key, value.ToString());
	}
}
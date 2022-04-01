using UnityEngine;
using UnityEngine.UI;
using System;

public class CountLimiter : MonoBehaviour {
	[SerializeField]
	private Toggle addBot;
	[Header("Fields")]
	[SerializeField]
	private InputField playerField;
	[SerializeField]
	private InputField botField;
	[Header("Limit")]
	[SerializeField]
	private int min;
	[SerializeField]
	private int max;

	private void Start() {
		playerField.text = GetText(PrefabsKeys.PLAYER_COUNT);
		botField.text = GetText(PrefabsKeys.BOT_COUNT);
	}
	private string GetText(string key) {
		return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key).ToString() : "10";
	}
	public void ChangePlayer(string value) {
		Change(value, playerField, botField, PrefabsKeys.PLAYER_COUNT);
	}
	public void ChangeBot(string value) {
		Change(value, botField, playerField, PrefabsKeys.BOT_COUNT);
	}
	private void Change(string value, InputField own, InputField other, string prefsKey) {
		int ownCount = Convert.ToInt32(value);
		if (addBot.isOn) {
			int otherCount = Convert.ToInt32(other.text);
			if (otherCount + ownCount > max) {
				ownCount = max - otherCount;
			}
		} else {
			if (ownCount < min + 1) {
				ownCount = min + 1;
			}
		}
		int res = Mathf.Clamp(ownCount, min, max);
		own.text = res.ToString();
		PlayerPrefs.SetInt(prefsKey, res);
	}
	public void ChangeToggle(bool value) {
		ChangePlayer(GetText(PrefabsKeys.PLAYER_COUNT));
	}
}
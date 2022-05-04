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
		playerField.text = GetText(LocalStorage.PLAYER_COUNT);
		botField.text = GetText(LocalStorage.BOT_COUNT);
	}
	private string GetText(string key) {
		return LocalStorage.GetValue(key, 2).ToString();
	}
	public void ChangePlayer(string value) {
		Change(value, playerField, botField, LocalStorage.PLAYER_COUNT);
	}
	public void ChangeBot(string value) {
		Change(value, botField, playerField, LocalStorage.BOT_COUNT);
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
		ChangePlayer(GetText(LocalStorage.PLAYER_COUNT));
	}
}
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
		playerField.text = GetText("player-count");
		botField.text = GetText("bot-count");
	}
	private string GetText(string key) {
		if (PlayerPrefs.HasKey(key)) {
			return PlayerPrefs.GetInt(key).ToString();
		}
		return "1";
	}
	public void ChangePlayer(String value) {
		Change(value, playerField, botField, "player-count");
	}
	public void ChangeBot(String value) {
		Change(value, botField, playerField, "bot-count");
	}
	private void Change(String value, InputField own, InputField other, string prefsKey) {
		int ownCount = Convert.ToInt32(value);
		if (addBot.isOn) {
			int otherCount = Convert.ToInt32(other.text);
			if (otherCount + ownCount > 6) {
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
		ChangePlayer(playerField.text);
	}
}
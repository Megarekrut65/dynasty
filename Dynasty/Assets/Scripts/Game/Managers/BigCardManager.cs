using System;
using UnityEngine;
using UnityEngine.UI;

public class BigCardManager {
	private const string BIG_CARD_KEY = "BigCard";
	private bool needMakeBig;
	public bool NeedMakeBig {
		get => needMakeBig;
		set {
			needMakeBig = value;
			PlayerPrefs.SetString(BIG_CARD_KEY, value.ToString());
		}
	}
	private GameObject cardPlace;

	public BigCardManager(GameObject cardPlace, Toggle toggle) {
		this.cardPlace = cardPlace;
		if (!PlayerPrefs.HasKey(BIG_CARD_KEY)) {
			PlayerPrefs.SetString(BIG_CARD_KEY, "false");
		}
		needMakeBig = Convert.ToBoolean(PlayerPrefs.GetString(BIG_CARD_KEY));
		toggle.isOn = needMakeBig;
		toggle.onValueChanged.AddListener(Change);
	}
	public void MakeBig(GameObject obj) {
		if (needMakeBig) obj.transform.SetParent(cardPlace.transform, false);
	}
	public void Change(bool value) {
		NeedMakeBig = value;
	}
}
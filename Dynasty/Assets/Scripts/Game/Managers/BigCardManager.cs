using System;
using UnityEngine;
using UnityEngine.UI;

public class BigCardManager {
	private const string bigCardKey = "BigCard";
	private bool needMakeBig;
	public bool NeedMakeBig {
		get {
			return needMakeBig;
		}
		set {
			needMakeBig = value;
			PlayerPrefs.SetString(bigCardKey, value.ToString());
		}
	}
	private GameObject cardPlace;
	private Toggle toggle;

	public BigCardManager(GameObject cardPlace, Toggle toggle) {
		this.cardPlace = cardPlace;
		if (!PlayerPrefs.HasKey(bigCardKey)) {
			PlayerPrefs.SetString(bigCardKey, "false");
		}
		needMakeBig = Convert.ToBoolean(PlayerPrefs.GetString(bigCardKey));
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
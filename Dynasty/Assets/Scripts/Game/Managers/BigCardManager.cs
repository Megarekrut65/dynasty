using System;
using UnityEngine;

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

	public BigCardManager(GameObject cardPlace) {
		this.cardPlace = cardPlace;
		if (!PlayerPrefs.HasKey(bigCardKey)) {
			PlayerPrefs.SetString(bigCardKey, "false");
		}
		needMakeBig = Convert.ToBoolean(PlayerPrefs.GetString(bigCardKey));
	}
	public void MakeBig(GameObject obj) {
		if (needMakeBig) obj.transform.SetParent(cardPlace.transform, false);
	}
}
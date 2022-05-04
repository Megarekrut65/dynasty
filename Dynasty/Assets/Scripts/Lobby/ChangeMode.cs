using System;
using UnityEngine;

public class ChangeMode : MonoBehaviour {
	[SerializeField]
	private GameMode gameMode;
	public void Change() {
		PlayerPrefs.SetString(LocalStorage.GAME_MODE, gameMode.ToString());
		PlayerPrefs.SetInt(LocalStorage.DESK_SEED, new System.Random().Next());
	}
}
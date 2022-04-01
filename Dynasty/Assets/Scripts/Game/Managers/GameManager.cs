using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField]
	private DependenciesManager dependenciesManager = new DependenciesManager();
	public bool GameOver { get; set; }
	public GameDependencies Dependencies => dependenciesManager.GetDependencies();

	public void ChangeMakeBig(bool value) {
		Dependencies.bigCardManager.NeedMakeBig = value;
	}

	private void Start() {
		Dependencies.logger.TranslatedLog(PlayerPrefs.HasKey(PrefabsKeys.GAME_MODE)?PlayerPrefs.GetString(PrefabsKeys.GAME_MODE):"offline");
	}
}
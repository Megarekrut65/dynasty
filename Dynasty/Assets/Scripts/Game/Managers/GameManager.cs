using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[SerializeField]
	private DependenciesManager dependenciesManager = new DependenciesManager();
	private bool gameOver;
	public bool GameOver {
		get {
			return gameOver;
		}
		set {
			gameOver = value;
		}
	}
	public GameDependencies Dependencies {
		get {
			return dependenciesManager.GetDependencies();
		}
	}
	public void ChangeMakeBig(bool value) {
		Dependencies.bigCardManager.NeedMakeBig = value;
	}
}
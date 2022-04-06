using UnityEngine;

public class ChangeMode : MonoBehaviour {
	[SerializeField]
	private GameMode gameMode;
	public void Change() {
		PlayerPrefs.SetString(PrefabsKeys.GAME_MODE, gameMode.ToString());
	}
}
using UnityEngine;

public class ChangeMode : MonoBehaviour{
	[SerializeField]
	private string mode;
	public void Change() {
		PlayerPrefs.SetString(PrefabsKeys.GAME_MODE, mode);
	}
}
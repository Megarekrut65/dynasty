using UnityEngine;

public class GameLogger : MonoBehaviour {
	public virtual void Log(string text) {
		Debug.Log(text);
	}
}
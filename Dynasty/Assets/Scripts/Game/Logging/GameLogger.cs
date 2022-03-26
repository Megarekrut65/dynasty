using UnityEngine;

public class GameLogger : MonoBehaviour {
	public virtual void Log(string text) {
		Debug.Log(text);
	}
	public void TranslatedLog(string keys) {
		Log(Translator.Translate(keys));
	}
}
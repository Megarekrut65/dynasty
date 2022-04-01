using UnityEngine;

public class MusicManager : MonoBehaviour {
	private static MusicManager _instance;
	private bool playNext = false;
	public bool PlayNext {
		set => playNext = value;
	}
	public static MusicManager Instance => _instance;

	private void Awake() {
		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			Destroy(gameObject);
		}
		LoadManager();
	}
	private void LoadManager() {
		if (playNext) {
			_instance.GetComponent<AudioSource>().Stop();
		} else if (!_instance.GetComponent<AudioSource>().isPlaying) {
			_instance.GetComponent<AudioSource>().Play();
		}
	}
	private void Start() {
		gameObject.GetComponent<AudioSource>().Play();
	}
}
using UnityEngine;

public class MusicManager : MonoBehaviour {
	private static MusicManager instance;
	private bool playNext = false;
	public bool PlayNext {
		set {
			playNext = value;
		}
	}
	public MusicManager Instance {
		get {
			return instance;
		}
	}

	private void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		LoadManager();
	}
	private void LoadManager() {
		if (playNext) {
			instance.GetComponent<AudioSource>().Stop();
		} else if (!instance.GetComponent<AudioSource>().isPlaying) {
			instance.GetComponent<AudioSource>().Play();
		}
	}
	private void Start() {
		gameObject.GetComponent<AudioSource>().Play();
	}
}
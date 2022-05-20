using UnityEngine;

public class MusicManager : MonoBehaviour {
	[SerializeField]
	private AudioSource audioSource;
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
		DontDestroyOnLoad(gameObject);
	}
	private void LoadManager() {
		_instance.Volume(LocalStorage.GetValue("music", 0.5f));
		if (playNext) {
			_instance.audioSource.Stop();
		} else if (!_instance.audioSource.isPlaying) {
			_instance.audioSource.Play();
		}
	}
	private void Start() {
		gameObject.GetComponent<AudioSource>().Play();
	}
	public void Volume(float value) {
		_instance.audioSource.volume = value;
	}
}
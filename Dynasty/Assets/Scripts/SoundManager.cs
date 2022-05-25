using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField]
    private AudioSource[] sources;
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else if (_instance != this) {
            Destroy(gameObject);
        }

        Volume(LocalStorage.GetValue("sound", 0.5f));
        DontDestroyOnLoad(gameObject);
    }
    public void Volume(float value) {
        foreach (var source in _instance.sources) {
            source.volume = value;
        }
    }
    public void Play(int index) {
        if (_instance.sources.Length > index && index >= 0) {
            _instance.sources[index].Play();
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour {
    [SerializeField]
    private string volumeType;
    [SerializeField]
    private Slider slider;

    private void Start() {
        float volume = LocalStorage.GetValue(volumeType, 0.5f);
        slider.value = volume;
        slider.onValueChanged.AddListener(Change);
    }
    private void Change(float value) {
        PlayerPrefs.SetFloat(volumeType, value);
        if (volumeType == "music") {
            MusicManager.Instance.Volume(value);
            return;
        }
        SoundManager.Instance.Volume(value);
    }
}

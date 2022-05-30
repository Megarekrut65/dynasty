using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that saves state of toggle in scene
/// </summary>
public class SavedToggle : MonoBehaviour {
    [SerializeField]
    protected Toggle toggle;
    protected string key = "";
    protected bool dif = false;

    protected virtual void Start() {
        toggle.onValueChanged.AddListener(Change);
        toggle.isOn = Convert.ToBoolean(LocalStorage.GetValue(key, dif.ToString()));
    }
    private void Change(bool value) {
        PlayerPrefs.SetString(key, value.ToString());
    }
}
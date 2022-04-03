using System;
using UnityEngine;
using UnityEngine.UI;

public class SavedToggle:MonoBehaviour {
    [SerializeField]
    protected Toggle toggle;
    protected string key = "";
    protected bool def = false;

    protected virtual void Start() {
        toggle.onValueChanged.AddListener(Change);
        toggle.isOn = Convert.ToBoolean(PrefabsKeys.GetValue(key, def.ToString()));
    }
    private void Change(bool value) {
        PlayerPrefs.SetString(key, value.ToString());
    }
}
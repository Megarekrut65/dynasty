using System;
using UnityEngine;
using UnityEngine.UI;

public class KeepPrivate : MonoBehaviour {
    [SerializeField]
    private Toggle toggle;

    private void Start() {
        toggle.onValueChanged.AddListener(Change);
        toggle.isOn = Convert.ToBoolean(PrefabsKeys.GetValue(PrefabsKeys.KEEP_PRIVATE, false.ToString()));
    }
    private void Change(bool value) {
        PlayerPrefs.SetString(PrefabsKeys.KEEP_PRIVATE, value.ToString());
    }
}
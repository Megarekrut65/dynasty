using System;
using UnityEngine;
using UnityEngine.UI;

public class KeepPrivate : MonoBehaviour {
    [SerializeField]
    private Toggle toggle;

    private void Start() {
        toggle.onValueChanged.AddListener(Change);
        toggle.isOn = PlayerPrefs.HasKey(PrefabsKeys.KEEP_PRIVATE) && 
                      Convert.ToBoolean(PlayerPrefs.GetString(PrefabsKeys.KEEP_PRIVATE));
    }
    private void Change(bool value) {
        PlayerPrefs.SetString(PrefabsKeys.KEEP_PRIVATE, value.ToString());
    }
}
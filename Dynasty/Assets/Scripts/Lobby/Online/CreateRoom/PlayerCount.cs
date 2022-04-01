using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCount : MonoBehaviour {
    [SerializeField]
    private InputField inputField;

    private void Start() {
        inputField.onValueChanged.AddListener(Change);
        inputField.text = PlayerPrefs.HasKey(PrefabsKeys.PLAYER_COUNT)
            ? PlayerPrefs.GetInt(PrefabsKeys.PLAYER_COUNT).ToString()
            : "2";
    }
    private void Change(string value) {
        PlayerPrefs.SetInt(PrefabsKeys.PLAYER_COUNT, Convert.ToInt32(value));
    }
}
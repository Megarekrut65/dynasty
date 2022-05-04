using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCount : MonoBehaviour {
    [SerializeField]
    private InputField inputField;

    private void Start() {
        inputField.onValueChanged.AddListener(Change);
        inputField.text = LocalStorage.GetValue(LocalStorage.PLAYER_COUNT, 2).ToString();
    }
    private void Change(string value) {
        PlayerPrefs.SetInt(LocalStorage.PLAYER_COUNT, Convert.ToInt32(value));
    }
}
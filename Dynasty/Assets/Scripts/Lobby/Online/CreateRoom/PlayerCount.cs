using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCount : MonoBehaviour {
    [SerializeField]
    private InputField inputField;

    private void Start() {
        inputField.onValueChanged.AddListener(Change);
        inputField.text = PrefabsKeys.GetValue(PrefabsKeys.PLAYER_COUNT, 2).ToString();
    }
    private void Change(string value) {
        PlayerPrefs.SetInt(PrefabsKeys.PLAYER_COUNT, Convert.ToInt32(value));
    }
}
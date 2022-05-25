using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyBots : MonoBehaviour {
    [SerializeField]
    private Dropdown dropdown;
    private List<string> keys = new List<string>();

    private void Start() {
        dropdown.options.ForEach(op => keys.Add(op.text));
        string val = LocalStorage.GetValue(LocalStorage.DIFFICULTY_BOTS, keys[0]);
        int index = keys.IndexOf(val);
        dropdown.value = index;
    }
    public void Change(int value) {
        PlayerPrefs.SetString(LocalStorage.DIFFICULTY_BOTS, keys[value]);
    }
}
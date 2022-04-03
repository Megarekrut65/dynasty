using System;
using UnityEngine;

public class SavedHideToggle : SavedToggle {
    [SerializeField]
    protected GameObject obj;

    protected override void Start() {
        toggle.onValueChanged.AddListener(SetActive);
        base.Start();
    }
    private void SetActive(bool value) {
        obj.SetActive(!value);
    }
}
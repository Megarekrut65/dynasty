using System;
using UnityEngine;

public class HideChat : SavedHideToggle {
    protected override void Start() {
        key = PrefabsKeys.HIDE_CHAT;
        base.Start();
    }
}
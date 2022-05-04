using System;
using UnityEngine;
using UnityEngine.UI;

public class KeepPrivate : SavedToggle {
    protected override void Start() {
        key = LocalStorage.KEEP_PRIVATE;
        base.Start();
    }
}
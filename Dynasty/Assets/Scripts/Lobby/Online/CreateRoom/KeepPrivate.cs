using System;
using UnityEngine;
using UnityEngine.UI;

public class KeepPrivate : SavedToggle {
    protected override void Start() {
        key = PrefabsKeys.KEEP_PRIVATE;
        base.Start();
    }
}
using UnityEngine;

public class HideChat : SavedHideToggle {
    [SerializeField]
    private OpeningButton openingButton;
    
    protected override void Start() {
        key = LocalStorage.HIDE_CHAT;
        base.Start();
    }
    protected override void SetActive(bool value) {
        openingButton.Change(false);
        base.SetActive(value);
    }
}
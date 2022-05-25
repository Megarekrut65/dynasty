public class HideChat : SavedHideToggle {
    protected override void Start() {
        key = LocalStorage.HIDE_CHAT;
        base.Start();
    }
}
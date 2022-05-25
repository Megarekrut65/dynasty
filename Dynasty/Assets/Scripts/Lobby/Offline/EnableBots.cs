public class EnableBots : SavedHideToggle {
    protected override void Start() {
        key = LocalStorage.ENABLE_BOTS;
        base.Start();
    }
}
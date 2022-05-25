public class KeepPrivate : SavedToggle {
    protected override void Start() {
        key = LocalStorage.KEEP_PRIVATE;
        base.Start();
    }
}
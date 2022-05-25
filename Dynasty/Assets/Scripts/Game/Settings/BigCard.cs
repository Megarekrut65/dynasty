public class BigCard : SavedToggle {
    protected override void Start() {
        key = LocalStorage.BIG_CARD;
        base.Start();
    }
}
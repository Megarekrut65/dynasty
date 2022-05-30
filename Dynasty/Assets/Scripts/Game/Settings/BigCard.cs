public class BigCard : SavedToggle {
    protected override void Start() {
        dif = true;
        key = LocalStorage.BIG_CARD;
        base.Start();
    }
}
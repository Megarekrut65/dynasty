using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CardEffect = System.Func<bool>;

public class CardClickEffect:IClick {
    private string key;
    private CardEffect click;
    private bool canClick;
    private ButtonEffect buttonEffect;
    private bool clicked = false;
    
    public CardClickEffect(string key, CardEffect click, bool canClick, Transform transform) {
        this.key = key;
        this.click = click;
        this.canClick = canClick;
        UnityEvent up = null;
        if (GameModeFunctions.IsMode(GameMode.ONLINE)) {
            up = new UnityEvent();
            up.AddListener(() => {
                if (clicked) return;
                clicked = true;
                DatabaseReferences.GetPlayerReference()
                    .Child(GameKeys.GAME_STATE)
                    .Child(key == "avoid-inevitable" ? GameKeys.AVOID_END : GameKeys.CARD_CLICK)
                    .SetValueAsync(true);
            });
        }

        buttonEffect = new ButtonEffect(transform, null, up, true, 3);
    }
    public void Down(PointerEventData eventData) {
        if ((canClick || eventData == null)
            && (eventData != null || GameModeFunctions.IsMode(GameMode.OFFLINE))) buttonEffect.Down();
    }
    public void Up(PointerEventData eventData) {
        if (canClick || eventData == null) {
            if (eventData != null || GameModeFunctions.IsMode(GameMode.OFFLINE)) buttonEffect.Up();
            bool avoid = click();
            if (key == "avoid-inevitable") clicked = avoid;
        }
    }
}
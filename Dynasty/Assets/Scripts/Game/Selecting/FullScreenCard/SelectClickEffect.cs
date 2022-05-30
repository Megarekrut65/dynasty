using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// Effects of card or player selecting
/// </summary>
public class SelectClickEffect:IClick {
    private int id;
    private Action<int> select;
    private bool canClick;
    private ButtonEffect buttonEffect;
    
    public SelectClickEffect(int id, bool isPlayer, Action<int> select, bool canClick, Transform transform) {
        this.id = id;
        this.select = select;
        this.canClick = canClick;
        UnityEvent up = null;
        if (GameModeFunctions.IsMode(GameMode.ONLINE)) {
            up = new UnityEvent();
            bool clicked = false;
            up.AddListener(() => {
                if (clicked) return;
                clicked = true;
                var reference = DatabaseReferences.GetPlayerReference().Child(GameKeys.GAME_STATE)
                    .Child(GameKeys.SELECTING)
                    .Child(isPlayer ? GameKeys.SELECT_PLAYER : GameKeys.SELECT_CARD);
                reference.SetValueAsync(id);
            });
        }

        buttonEffect = new ButtonEffect(transform, null, up, true, isPlayer ? 1 : 4);
    }
    public bool Down(PointerEventData eventData) {
        if ((canClick || eventData == null)
            && (eventData != null || GameModeFunctions.IsMode(GameMode.OFFLINE))) {
            buttonEffect.Down();
            return true;
        }

        return false;
    }
    public bool Up(PointerEventData eventData) {
        if (canClick || eventData == null) {
            if (eventData != null || GameModeFunctions.IsMode(GameMode.OFFLINE)) buttonEffect.Up();
            select(id);
            return true;
        }

        return false;
    }
}
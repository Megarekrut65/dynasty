using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectClick : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	private int id;
	public int Id {
		set => id = value;
	}
	private bool isPlayer;
	public bool IsPlayer {
		set => isPlayer = value;
	}
	private Action<int> select;
	public Action<int> Select {
		set => select = value;
	}
	private bool canClick;
	public bool CanClick {
		set => canClick = value;
	}
	private ButtonEffect buttonEffect;
	private void Start() {
		UnityEvent up = null;
		if (GameModeFunctions.IsMode(GameMode.ONLINE)) {
			up = new UnityEvent();
			bool clicked = false;
			up.AddListener(() => {
				if(clicked) return;
				clicked = true;
				var reference = DatabaseReferences.GetPlayerReference().Child(GameKeys.GAME_STATE)
					.Child(GameKeys.SELECTING)
					.Child(isPlayer ? GameKeys.SELECT_PLAYER : GameKeys.SELECT_CARD);
				reference.SetValueAsync(id);
			});
		}
		buttonEffect = new ButtonEffect(transform, null,up);
	}
	public void OnPointerDown(PointerEventData eventData) {
		//if (canClick || eventData == null) buttonEffect.Down();
	}
	public void OnPointerUp(PointerEventData eventData) {
		if (canClick || eventData == null) {
			if(eventData != null) buttonEffect.Up();
			select(id);
		}
	}
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using CardEffect = System.Func<bool>;

public class CardClick : MonoBehaviour,
	IPointerDownHandler, IPointerUpHandler {
	public string Key { get; set; }
	private CardEffect click;
	public CardEffect Click {
		set => click = value;
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
				DatabaseReferences.GetPlayerReference()
					.Child(GameKeys.GAME_STATE)
					.Child(Key == "avoid-inevitable"?GameKeys.AVOID_END:GameKeys.CARD_CLICK)
					.SetValueAsync(true);
			});
		}
		buttonEffect = new ButtonEffect(transform, null,up, true, 3);
	}
	public void OnPointerDown(PointerEventData eventData) {
		if (canClick || eventData == null) buttonEffect.Down();
	}
	public void OnPointerUp(PointerEventData eventData) {
		if (canClick || eventData == null) {
			if(eventData != null) buttonEffect.Up();
			click();
		}
	}
}
using UnityEngine;
using UnityEngine.EventSystems;
public class DeskManager : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	[SerializeField]
	private TableManager manager;
	[SerializeField]
	private GameManager gameManager;
	[SerializeField]
	private Animation anim;
	void Awake() {
		gameManager.Next += Next;
	}
	public void OnPointerDown(PointerEventData eventData) {
	}
	public void OnPointerUp(PointerEventData eventData) {
		if (manager.PlayerRound()) {
			anim.Stop("DeskActive");
			manager.TakeCardFromDesk();
		}
	}
	protected void Next() {
		if (manager.PlayerRound()) anim.Play("DeskActive");
	}
}
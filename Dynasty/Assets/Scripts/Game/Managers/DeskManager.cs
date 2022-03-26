using UnityEngine;
using UnityEngine.EventSystems;
public class DeskManager : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	[SerializeField]
	private TableManager manager;
	[SerializeField]
	private GameManager gameManager;
	[SerializeField]
	private Animation deskAnimation;

	void Awake() {
		gameManager.Dependencies.roundManager.Next += Next;
	}
	public void OnPointerDown(PointerEventData eventData) {
	}
	public void OnPointerUp(PointerEventData eventData) {
		if (manager.PlayerRound()) {
			deskAnimation.Stop("DeskActive");
			manager.TakeCardFromDesk();
		}
	}
	protected void Next() {
		if (manager.PlayerRound()) {
			deskAnimation.Play("DeskActive");
			gameManager.Dependencies.logger.TranslatedLog(
				$"turn of {gameManager.Dependencies.roundManager.WhoIsNextPlayer().nickname}");
		}
	}
}
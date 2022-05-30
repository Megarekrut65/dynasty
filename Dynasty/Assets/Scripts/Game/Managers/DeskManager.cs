using UnityEngine;
using UnityEngine.EventSystems;

public class DeskManager : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Animation deskAnimation;
    private bool canTake = false;

    private void Awake() {
        gameManager.GameDependencies.roundManager.Next += Next;
    }
    private void OnDestroy() {
        gameManager.GameDependencies.roundManager.Next -= Next;
    }
    public void OnPointerDown(PointerEventData eventData) {
    }
    public void OnPointerUp(PointerEventData eventData) {
        if (canTake) {
            canTake = false;
            deskAnimation.Stop("DeskActive");
            if(GameModeFunctions.IsMode(GameMode.ONLINE)) DatabaseReferences.GetPlayerReference()
                .Child(GameKeys.GAME_STATE)
                .Child(GameKeys.IS_CLICK)
                .SetValueAsync(true);
            gameManager.CardTaker.TakeCardFromDesk();
        }
    }

    private void Next() {
        if (gameManager.CardTaker.PlayerRound()) {
            canTake = true;
            deskAnimation.Play("DeskActive");
            gameManager.GameDependencies.logger.TranslatedLog(
                $"turn of {gameManager.GameDependencies.roundManager.WhoIsNextPlayer().Nickname}");
        }
    }
}
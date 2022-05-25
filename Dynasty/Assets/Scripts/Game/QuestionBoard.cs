using UnityEngine;

public class QuestionBoard : MonoBehaviour {
    [SerializeField]
    private GameManager gameManager;

    public void Ok() {
        gameManager.Leave();
    }
    public void Cancel() {
        gameObject.SetActive(false);
    }
}
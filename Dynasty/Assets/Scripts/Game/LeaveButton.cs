using UnityEngine;

public class LeaveButton : MonoBehaviour {
    [SerializeField]
    private GameObject questionBoard;

    public void Leave() {
        questionBoard.SetActive(true);
    }
}
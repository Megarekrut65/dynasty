using System.Collections;
using UnityEngine;

public class ToastBoard {
    private GameObject board = null;
    private TextBoardObject text;

    public ToastBoard(GameObject toast, GameObject canvas) {
        if (toast != null && canvas != null) {
            board = Object.Instantiate(toast, new Vector3(0, 0, 0), Quaternion.identity);
            board.transform.SetParent(canvas.transform, false);
            text = board.GetComponent<TextBoardObject>();
            board.SetActive(false);
        }
    }
    public void ShowMessage(string message, float seconds) {
        text.SetText(message);
        board.SetActive(true);
        text.StartCoroutine(Hide(seconds));
    }
    private IEnumerator Hide(float seconds) {
        yield return new WaitForSeconds(seconds);
        board.SetActive(false);
    }
}
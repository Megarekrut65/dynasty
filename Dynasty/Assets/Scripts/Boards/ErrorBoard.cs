using UnityEngine;

public class ErrorBoard {
    private GameObject board = null;
    private TextBoardObject text;
    public ErrorBoard(GameObject redBoard, GameObject canvas) {
        if (redBoard != null && canvas != null) {
            board = Object.Instantiate(redBoard, new Vector3(0, 0, 0), Quaternion.identity);
            board.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            board.transform.SetParent(canvas.transform, false);
            text = board.GetComponent<TextBoardObject>();
            SetActive(false);
        }
    }
    public void SetActive(bool value) {
        board.SetActive(value);
    }
    public void SetMessage(string message) {
        text.SetText(message);
    }
}
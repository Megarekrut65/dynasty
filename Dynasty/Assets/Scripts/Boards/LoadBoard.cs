using UnityEngine;

public class LoadBoard {
    private GameObject board = null;

    public LoadBoard(GameObject blackBoard, GameObject canvas) {
        if (blackBoard != null && canvas != null) {
            board = Object.Instantiate(blackBoard, new Vector3(0, 0, 0), Quaternion.identity);
            board.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            board.transform.SetParent(canvas.transform, false);
            SetActive(false);
        }
    }
    public void SetActive(bool value) {
        board.SetActive(value);
    }
}
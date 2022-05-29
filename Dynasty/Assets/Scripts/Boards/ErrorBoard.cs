using UnityEngine;

/// <summary>
/// Control class for prefab with red background and text object that is used for error messaging
/// </summary>
public class ErrorBoard {
    /// <summary>Prefab to create in scene</summary>
    private GameObject board;
    /// <summary>Text of board</summary>
    private TextBoardObject text;

    /// <summary>
    /// Instantiates red board in canvas
    /// </summary>
    /// <param name="redBoard">GameObject with red background and error message</param>
    /// <param name="canvas">Canvas in scene</param>
    public ErrorBoard(GameObject redBoard, GameObject canvas) {
        if (redBoard != null && canvas != null) {
            board = Object.Instantiate(redBoard, new Vector3(0, 0, 0), Quaternion.identity);
            board.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            board.transform.SetParent(canvas.transform, false);
            text = board.GetComponent<TextBoardObject>();
            SetActive(false);
        }
    }
    /// <summary>
    /// Show and hides board
    /// </summary>
    /// <param name="value">True to show board and false to hide</param>
    public void SetActive(bool value) {
        board.SetActive(value);
    }
    /// <summary>
    /// Changes message of error
    /// </summary>
    /// <param name="message">New error message</param>
    public void SetMessage(string message) {
        text.SetText(message);
    }
}
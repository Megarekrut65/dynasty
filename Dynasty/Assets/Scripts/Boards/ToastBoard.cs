using System.Collections;
using UnityEngine;

/// <summary>
/// Control class for messaging like Toast in Android Studio
/// </summary>
public class ToastBoard {
    /// <summary>
    /// Prefab to create in scene
    /// </summary>
    private GameObject board;
    /// <summary>
    /// Text of board
    /// </summary>
    private TextBoardObject text;

    /// <summary>
    /// Instantiates toast board in canvas
    /// </summary>
    /// <param name="toast">GameObject with text object in the bottom</param>
    /// <param name="canvas">Canvas in scene</param>
    public ToastBoard(GameObject toast, GameObject canvas) {
        if (toast != null && canvas != null) {
            board = Object.Instantiate(toast, new Vector3(0, 0, 0), Quaternion.identity);
            board.transform.SetParent(canvas.transform, false);
            text = board.GetComponent<TextBoardObject>();
            board.SetActive(false);
        }
    }
    /// <summary>
    /// Shows message in bottom of screen for some time
    /// </summary>
    /// <param name="message">Text to show</param>
    /// <param name="seconds">Time of showing</param>
    public void ShowMessage(string message, float seconds) {
        text.SetText(message);
        board.SetActive(true);
        text.StartCoroutine(Hide(seconds));
    }
    /// <summary>
    /// Hides message after some time
    /// </summary>
    /// <param name="seconds">Time of showing</param>
    /// <returns></returns>
    private IEnumerator Hide(float seconds) {
        yield return new WaitForSeconds(seconds);
        board.SetActive(false);
    }
}
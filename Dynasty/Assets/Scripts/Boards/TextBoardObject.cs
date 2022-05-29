using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for control text object in board
/// </summary>
public class TextBoardObject : MonoBehaviour {
    /// <summary>
    /// Text object
    /// </summary>
    [SerializeField]
    private Text text;
    
    /// <summary>
    /// Hides this board
    /// </summary>
    public void HideBoard() {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Changes text value
    /// </summary>
    /// <param name="value">Value to set in text object</param>
    public void SetText(string value) {
        text.text = value;
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for button object that open scene by name
/// </summary>
public class OpenScene : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private string sceneName;

    public void OnPointerUp(PointerEventData eventData) {
    }
    public void OnPointerDown(PointerEventData eventData) {
        SceneManager.LoadScene(this.sceneName, LoadSceneMode.Single);
    }
}
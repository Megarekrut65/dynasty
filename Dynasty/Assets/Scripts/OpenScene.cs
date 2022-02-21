using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OpenScene : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private string sceneName;
    public void OnPointerUp(PointerEventData eventData)
    {
    }
    public void OnPointerDown(PointerEventData eventData){
        SceneManager.LoadScene(this.sceneName, LoadSceneMode.Single);
    }
}

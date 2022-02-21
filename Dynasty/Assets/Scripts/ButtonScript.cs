using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonScript : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler
{
    private Vector3 scale;
    [SerializeField]
    private GameObject soundClick;
    private void Start() {
        scale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {   
        transform.localScale = 1.1f * transform.localScale;
        if(soundClick != null) soundClick.GetComponent<AudioSource>().Play();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = transform.localScale/1.1f;
    }
}

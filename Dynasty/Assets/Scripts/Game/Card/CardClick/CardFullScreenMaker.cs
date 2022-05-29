using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardFullScreenMaker : MonoBehaviour {
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private CardLoader loader;
    [SerializeField]
    private CardFullScreenClick cardFullScreenClick;
    [SerializeField]
    private Outline outline;

    private void Start() {
        cardFullScreenClick.HideBackground = Hide;
    }
    public void TakeCard(string key, IClick click, Color color) {
        loader.Key = key;
        loader.LoadData();
        cardFullScreenClick.Click = click;
        if (color.a > 0f) color.a = 0.5f;
        outline.effectColor = color;
        background.SetActive(true);
    }
    public void ClickOnCardDown(PointerEventData eventData) {
        cardFullScreenClick.OnPointerDown(eventData);
    }
    public void ClickOnCardUp(PointerEventData eventData) {
        cardFullScreenClick.OnPointerUp(eventData);
    }
    public void Hide() {
        background.SetActive(false);
    }
}
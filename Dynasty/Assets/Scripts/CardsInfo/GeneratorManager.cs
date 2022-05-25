using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorManager : MonoBehaviour {
    [SerializeField]
    private GameObject cardObject;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private GameObject blackBoard;
    private GameObject canvas;
    private List<GameObject> cards;
    private ResizingData data = new ResizingData(1000);
    private int currentColor = -2;
    private string currentName = "";

    private void Start() {
        canvas = GameObject.Find("Canvas");
        StartCoroutine(Generate());
    }
    private IEnumerator Generate() {
        LoadBoard loadBoard = new LoadBoard(blackBoard, canvas);
        loadBoard.SetActive(true);
        yield return new WaitForSeconds(0.001f);
        CardsGenerator generator = new CardsGenerator(cardObject, content);
        cards = generator.Generate();
        foreach (var card in cards) {
            card.GetComponent<CardLoader>().LoadData();
            yield return new WaitForSeconds(0.01f);
            card.GetComponent<ResizingTextCard>().Resize(data);
            yield return new WaitForSeconds(0.05f);
        }

        currentColor = -2;
        yield return new WaitForSeconds(0.1f);
        loadBoard.SetActive(false);
    }

    private void MakeInvisible() {
        foreach (var card in cards) {
            card.SetActive(false);
        }

        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    public void SelectColor(int value) {
        currentColor = value - 2;
        Filter();
    }
    public void SelectName(string cardName) {
        currentName = cardName;
        Filter();
    }
    private void Filter() {
        MakeInvisible();
        foreach (var card in cards) {
            CardData cardData = card.GetComponent<CardLoader>().Card;
            if ((Math.Sign(cardData.amount) == currentColor || currentColor == -2)
                && cardData.name.ToLower().Contains(currentName.ToLower())) {
                card.SetActive(true);
            }
        }

        scrollRect.normalizedPosition = new Vector2(0, 0);
        scrollRect.horizontalNormalizedPosition = 0;
    }
}
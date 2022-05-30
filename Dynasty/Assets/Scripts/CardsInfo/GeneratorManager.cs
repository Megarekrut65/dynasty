using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates all cards in scene
/// </summary>
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
    /// <summary>
    /// Generates all cards and loads all card data
    /// </summary>
    /// <returns></returns>
    private IEnumerator Generate() {
        LoadBoard loadBoard = new LoadBoard(blackBoard, canvas);
        loadBoard.SetActive(true);
        yield return new WaitForSeconds(0.001f);
        CardsGenerator generator = new CardsGenerator(cardObject);
        cards = generator.Generate();
        foreach (var card in cards) card.transform.SetParent(content.transform, false);
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
    /// <summary>
    /// Hides all cards
    /// </summary>
    private void MakeInvisible() {
        foreach (var card in cards) {
            card.SetActive(false);
        }

        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    /// <summary>
    /// Changes current color
    /// </summary>
    /// <param name="value"></param>
    public void SelectColor(int value) {
        currentColor = value - 2;
        Filter();
    }
    /// <summary>
    /// Changes name pattern
    /// </summary>
    /// <param name="cardName">new name pattern</param>
    public void SelectName(string cardName) {
        currentName = cardName;
        Filter();
    }
    /// <summary>
    /// Filters cards by name and color
    /// </summary>
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
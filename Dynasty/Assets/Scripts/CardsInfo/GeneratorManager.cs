using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System;
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
    private GameObject _canvas;
    private List<GameObject> cards;
    private ResizingData data = new ResizingData(1000);

    void Start(){
        _canvas = GameObject.Find("Canvas");
        StartCoroutine(Generate());
    }
    IEnumerator Generate(){
        LoadBoard loadBoard = new LoadBoard(blackBoard, _canvas);
        yield return new WaitForSeconds(0.5f);
        CardsGenerator generator = new CardsGenerator(cardObject, content);
        cards = generator.Generate();
        yield return new WaitForSeconds(0.5f);
        foreach(var card in cards){
            yield return null;
            card.GetComponent<LocalizationCard>().UpdateText();
            card.GetComponent<ResizingTextCard>().Resize(data);
        }
        SelectColor(2);
        loadBoard.Destroy();
    }
    public void MakeInvisible(){
        foreach(var card in cards){
            card.SetActive(false);
        }
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    public void SelectColor(int value){
        MakeInvisible();
        foreach(var card in cards){
            if(Math.Sign(card.GetComponent<LocalizationCard>().Card.amount) == value - 1){
                card.SetActive(true);
            }
        }
        scrollRect.normalizedPosition = new Vector2(0, 0);
        scrollRect.horizontalNormalizedPosition = 0;
    }
}
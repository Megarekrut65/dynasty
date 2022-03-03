using System.Threading;
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
    private int currentColor = -2;
    private string currentName = "";

    void Start(){
        _canvas = GameObject.Find("Canvas");
        StartCoroutine(Generate());
    }
    IEnumerator Generate(){
        LoadBoard loadBoard = new LoadBoard(blackBoard, _canvas);
        yield return new WaitForSeconds(0.001f);
        CardsGenerator generator = new CardsGenerator(cardObject, content);
        cards = generator.Generate();
        foreach(var card in cards){
            card.GetComponent<LocalizationCard>().UpdateText();
            yield return new WaitForSeconds(0.001f);
            card.GetComponent<ResizingTextCard>().Resize(data);
        }
        SelectColor(0);
        loadBoard.Destroy();
    }
    public void MakeInvisible(){
        foreach(var card in cards){
            card.SetActive(false);
        }
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    public void SelectColor(int value){
        currentColor = value - 2;
        Filter();
    }
    public void SelectName(String name){
        currentName = name;
        Filter();
    }
    private void Filter(){
        MakeInvisible();
        foreach(var card in cards){
            CardData data = card.GetComponent<LocalizationCard>().Card;
            if( (Math.Sign(data.amount) == currentColor || currentColor == -2)
            && data.name.ToLower().Contains(currentName.ToLower())){
                card.SetActive(true);
            }
        }
        scrollRect.normalizedPosition = new Vector2(0, 0);
        scrollRect.horizontalNormalizedPosition = 0;
    }
}
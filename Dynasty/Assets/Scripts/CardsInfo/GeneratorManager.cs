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

    void Start(){
        _canvas = GameObject.Find("Canvas");
        StartCoroutine(Generate());
    }
    IEnumerator Generate(){
        LoadBoard loadBoard = new LoadBoard(blackBoard, _canvas);
        CardsGenerator generator = new CardsGenerator(cardObject, content);
        cards = generator.Generate();
        scrollRect.normalizedPosition = new Vector2(0, 0);
        yield return new WaitForSeconds(0.5f);
        loadBoard.Destroy();
    }
}
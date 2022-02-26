using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardsGenerator : MonoBehaviour {
    [SerializeField]
    private GameObject cardObject;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private GameObject blackBoard;
    private GameObject _canvas;

    void Start(){
        _canvas = GameObject.Find("Canvas");
        StartCoroutine(Generate());
    }
    IEnumerator Generate(){
        LoadBoard loadBoard = new LoadBoard(blackBoard, _canvas);
        var keys = LocalizationMap<CardData>.GetInstance().Map.Keys;
        foreach(var key in keys){
            GameObject obj = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            obj.GetComponent<LocalizationCard>().Key = key;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2 (305f/2, 495f/2);
            obj.transform.SetParent(content.transform, false);
        }
        scrollRect.normalizedPosition = new Vector2(0, 0);
        yield return new WaitForSeconds(0.5f);
        loadBoard.Destroy();
    }
}
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardsGenerator{
    private GameObject cardObject;
    private GameObject content;
    public CardsGenerator(GameObject cardObject, GameObject content){
        this.cardObject = cardObject;
        this.content = content;
    }
    public List<GameObject> Generate(){
        List<GameObject> list = new List<GameObject>();
        foreach(var item in LocalizationManager.instance.map.CardMap){
            GameObject obj = MonoBehaviour.Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            obj.GetComponent<LocalizationCard>().Key = item.Key;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2 (305f/2, 495f/2);
            obj.transform.SetParent(content.transform, false);
            list.Add(obj);
        }
        return list;
    }
}
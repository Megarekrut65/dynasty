using System.Collections.Generic;
using UnityEngine;

public class CardsGenerator {
    private GameObject cardObject;
    public CardsGenerator(GameObject cardObject) {
        this.cardObject = cardObject;
    }
    public List<GameObject> Generate() {
        List<GameObject> list = new List<GameObject>();
        var map = LocalizationManager.Instance.map.CardMap;
        List<string> keys = new List<string>(map.Keys);
        keys.Sort((item1, item2) => map[item2].amount.CompareTo(map[item1].amount));
        foreach (var key in keys) {
            GameObject obj = Object.Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            obj.GetComponent<CardLoader>().Key = key;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(305f / 2, 495f / 2);
            list.Add(obj);
        }

        return list;
    }
}
using System;
using UnityEngine;

[Serializable]
public class Card {
    public string key;
    public CardData data;
    public string type;
    private static int _id = 0;
    public int id;
    public GameObject obj = null;
    public bool needSelect = false;
    public Card underCard = null;
    public Card(CardData data, string key) {
        id = _id++;
        this.key = key;
        this.data = data;
        type = CardType.GetType(data);
    }
}
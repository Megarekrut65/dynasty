using UnityEngine;

public class Card{
    public string key;
    public CardData data;
    public string type;
    public Player owner;
    private static int _id = 0;
    public int id;
    
    public Card(CardData data, string key, Player owner = null){
        id = _id++;
        this.key = key;
        this.owner = owner;
        this.data = data;
        type = CardType.GetType(data);
    }
}
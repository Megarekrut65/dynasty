using UnityEngine;

public class Card {
	public string key;
	public CardData data;
	public string type;
	private static int _id = 0;
	public int id;
	public GameObject obj = null;

	public Card(CardData data, string key) {
		id = _id++;
		this.key = key;
		this.data = data;
		type = CardType.GetType(data);
	}
}
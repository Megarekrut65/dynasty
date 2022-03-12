using System;
public class CardData {
	public string key;
	public string name;
	public string description;
	public string type;
	public int count;
	public string move;
	public string mix;
	public int amount;
	public string cover;
	public string drop;

	public override string ToString() {
		return "name: {" + name + "}, description:{" + description + "}";
	}
	public CardData(CardInfo info, CardParameters parameters) {
		this.name = info.name;
		this.description = info.description;
		this.type = parameters.type;
		this.count = parameters.count;
		this.amount = parameters.amount;
		this.cover = parameters.cover;
		this.move = parameters.move;
		this.mix = parameters.mix;
		this.drop = parameters.drop;
	}
	public CardData(string key, CardParameters parameters) {
		this.key = key;
		this.type = parameters.type;
		this.count = parameters.count;
		this.amount = parameters.amount;
		this.cover = parameters.cover;
		this.move = parameters.move;
		this.mix = parameters.mix;
		this.drop = parameters.drop;
	}
	public void Change(CardInfo info) {
		this.name = info.name;
		this.description = info.description;
	}
}
using UnityEngine;

public class CardFunctions {
	public const string MIX = "mix";
	public const string MOVE = "move";
	public const string COVER = "cover";
	public const string DROP = "drop";
	public static string GetIconValue(string icon, Card card) {
		switch (icon) {
			case MIX: return card.data.mix;
			case MOVE: return card.data.move;
			case COVER: return card.data.cover;
			case DROP: return card.data.drop;
			default: return "none";
		}
	}
}
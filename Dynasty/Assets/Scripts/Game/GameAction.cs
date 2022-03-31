using System;

public static class GameAction {
	public static readonly Action Empty = () => { };
	public static Predicate<Card> GetAllFilter(string icon) {
		return card => CardFunctions.GetIconValue(icon, card) != "no";
	}
	public static Predicate<Card> GetFilter(string icon, Predicate<Card> predicate) {
		return card => CardFunctions.GetIconValue(icon, card) != "no" && predicate(card);
	}
}
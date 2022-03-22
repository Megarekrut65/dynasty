
using System;
public class GameAction {
	public static readonly Action EMPTY = () => { };
	public static Predicate<Card> GetAllFilter(string icon) {
		return (card) => CardFunctions.GetIconValue(icon, card) != "no";
	}
	public static Predicate<Card> GetFilter(string icon, Predicate<Card> predicate) {
		return (card) => CardFunctions.GetIconValue(icon, card) != "no" && predicate(card);
	}
}
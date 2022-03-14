
using System;
public class GameAction {
	public static readonly Action EMPTY = () => { };
	public static Predicate<Card> GetAllFilter(string icon) {
		return (card) => CardFunctions.GetIconValue(icon, card) != "no";
	}
}
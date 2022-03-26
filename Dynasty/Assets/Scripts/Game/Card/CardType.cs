
class CardType {
	public const string NONE = "none";
	public const string MONSTER = "monster";
	public const string KNIGHT = "knight";
	public const string WALL = "wall";
	public const string BUILDING = "building";
	public static string GetType(CardData data) {
		string name = data.name.ToLower();
		string description = data.description;
		if (name.Contains(Wall)) {
			return CardType.WALL;
		}
		if (description.Contains(Monster)) {
			return CardType.MONSTER;
		}
		if (description.Contains(Knight)) {
			return CardType.KNIGHT;
		}
		if (description.Contains(Building)) {
			return CardType.BUILDING;
		}
		return CardType.NONE;
	}
	private static string Monster {
		get {
			return LocalizationManager.Instance.GetWord(CardType.MONSTER);
		}
	}
	private static string Knight {
		get {
			return LocalizationManager.Instance.GetWord(CardType.KNIGHT);
		}
	}
	private static string Wall {
		get {
			return LocalizationManager.Instance.GetWord(CardType.WALL);
		}
	}
	private static string Building {
		get {
			return LocalizationManager.Instance.GetWord(CardType.BUILDING);
		}
	}
}
static class CardType {
    public const string NONE = "none";
    public const string MONSTER = "monster";
    public const string KNIGHT = "knight";
    public const string WALL = "wall";
    public const string BUILDING = "building";

    public static string GetType(CardData data) {
        switch (data.key) {
            case "air-wall":
            case "flame-wall":
            case "water-wall":
            case "earthen-wall":
                return WALL;
            case "swordsman":
            case "crusader":
            case "equestrian":
            case "archer":
                return KNIGHT;
            case "snake":
            case "crow":
            case "cerberus":
            case "locusts":
            case "siren":
            case "bear":
            case "hydra":
            case "zombie":
            case "slime":
            case "bat":
            case "black-cat":
                return MONSTER;
            case "temple":
            case "bunker":
            case "gate":
            case "village":
            case "castle":
            case "fountain":
            case "tower":
            case "bridge":
            case "prison":
            case "black-sun-castle":
            case "hut":
                return BUILDING;
            default:
                return NONE;
        }
    }
    /*public static string GetType(CardData data) {
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
    }*/
}
/// <summary>
/// Gets type of card by key
/// </summary>
public static class CardType {
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
}
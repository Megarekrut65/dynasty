
class CardType
{
    private const string NONE = "none";
    private const string MONSTER = "monster";
    private const string KNIGHT = "knight";
    private const string WALL = "wall";
    private const string BUILDING = "building";
    public static string GetType(CardData data)
    {
        string name = data.name.ToLower();
        string description = data.description;
        if (name.Contains(LocalizationManager.instance.GetWord(CardType.WALL)))
        {
            return CardType.WALL;
        }
        if (description.Contains(LocalizationManager.instance.GetWord(CardType.MONSTER)))
        {
            return CardType.MONSTER;
        }
        if (description.Contains(LocalizationManager.instance.GetWord(CardType.KNIGHT)))
        {
            return CardType.KNIGHT;
        }
        if (description.Contains(LocalizationManager.instance.GetWord(CardType.BUILDING)))
        {
            return CardType.BUILDING;
        }
        return CardType.NONE;
    }
    public static string None
    {
        get
        {
            return LocalizationManager.instance.GetWord(CardType.NONE);
        }
    }
    public static string Monster
    {
        get
        {
            return LocalizationManager.instance.GetWord(CardType.MONSTER);
        }
    }
    public static string Knight
    {
        get
        {
            return LocalizationManager.instance.GetWord(CardType.KNIGHT);
        }
    }
    public static string Wall
    {
        get
        {
            return LocalizationManager.instance.GetWord(CardType.WALL);
        }
    }
    public static string Building
    {
        get
        {
            return LocalizationManager.instance.GetWord(CardType.BUILDING);
        }
    }
}
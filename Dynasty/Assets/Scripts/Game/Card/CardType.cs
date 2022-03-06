
class CardType{
    public const string NONE="none";
    public const string MONSTER="monster";
    public const string KNIGHT = "knight";
    public const string WALL="wall";
    public const string BUILDING="building";
    public static string GetType(CardData data){
        string name = data.name.ToLower();
        string description = data.description.ToLower();
        if(name.Contains(LocalizationManager.instance.GetWord(CardType.WALL))){
            return CardType.WALL;
        }
        if(description.Contains(LocalizationManager.instance.GetWord(CardType.MONSTER))){
            return CardType.MONSTER;
        }
        if(description.Contains(LocalizationManager.instance.GetWord(CardType.KNIGHT))){
            return CardType.KNIGHT;
        }
        if(description.Contains(LocalizationManager.instance.GetWord(CardType.BUILDING))){
            return CardType.BUILDING;
        }
        return CardType.NONE;
    }
}
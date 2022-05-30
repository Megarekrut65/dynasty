public static class CardFunctions {
    public const string MIX = "mix";
    public const string MOVE = "move";
    public const string COVER = "cover";
    public const string DROP = "drop";
    
    /// <summary>
    /// Gets value of card icon
    /// </summary>
    /// <param name="icon">Icon type</param>
    /// <returns>value of card icon</returns>
    public static string GetIconValue(string icon, Card card) {
        return icon switch {
            MIX => card.data.mix,
            MOVE => card.data.move,
            COVER => card.data.cover,
            DROP => card.data.drop,
            _ => "none"
        };
    }
}
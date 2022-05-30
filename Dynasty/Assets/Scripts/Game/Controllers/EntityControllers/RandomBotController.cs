using System;
using Random = UnityEngine.Random;

/// <summary>
/// Implementation of bot controller. Select random card and player
/// </summary>
public class RandomBotController : BotController {
    public RandomBotController(Player player, GameDependencies dependencies,
        Table table, Func<Card> takeCard) : base(player, dependencies, table, takeCard) {
    }

    protected override SelectObjectData<Card> SelectCard() {
        var data = SelectManager.SelectData.selectingCards;
        return data.Count == 0 ? null : data[Random.Range(0, data.Count)];
    }
}
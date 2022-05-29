using System;
using Random = UnityEngine.Random;

public class RandomBotController : BotController {
    public RandomBotController(Player player, GameDependencies dependencies,
        Table table,CardFullScreenMaker cardFullScreenMaker, Func<Card> takeCard) 
        : base(player, dependencies, table,cardFullScreenMaker, takeCard) {
    }

    protected override SelectObjectData<Card> SelectCard() {
        var data = SelectManager.SelectData.selectingCards;
        return data.Count == 0 ? null : data[Random.Range(0, data.Count)];
    }
}
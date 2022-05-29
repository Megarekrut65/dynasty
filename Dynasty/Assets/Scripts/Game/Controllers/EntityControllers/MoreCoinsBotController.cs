using System;

public class MoreCoinsBotController : BotController {
    public MoreCoinsBotController(Player player, GameDependencies dependencies,
        Table table,CardFullScreenMaker cardFullScreenMaker, Func<Card> takeCard) 
        : base(player, dependencies, table,cardFullScreenMaker, takeCard) {
    }

    protected override SelectObjectData<Card> SelectCard() {
        return SelectManager.SelectData.selectingCards.Count == 0 ? null : FindBestSelect();
    }

    private SelectObjectData<Card> FindBestSelect() {
        var type = SelectManager.SelectData.lastType;
        return type switch {
            "mix" => FindBest(CountMixAmount),
            "move" => FindBest(CountMoveAmount),
            "cover" => FindBest(CountCoverAmount),
            "drop" => FindBest(CountDropAmount),
            _ => null
        };
    }
    private float CountMixAmount(SelectObjectData<Card> card) {
        return CountDropAmount(card);
    }
    private float CountMoveAmount(SelectObjectData<Card> card) {
        float coefficient = -1f;
        if (SelectManager.SelectData.toOwner) {
            coefficient = 0f;
        }

        return CountAmount(card, coefficient);
    }
    private float CountCoverAmount(SelectObjectData<Card> card) {
        float coefficient = -2f;
        if (table.Current.data.amount < 0) {
            coefficient = -0.5f;
        }

        return CountAmount(card, coefficient);
    }
    private float CountDropAmount(SelectObjectData<Card> card) {
        return CountAmount(card, -2.5f);
    }
    private float CountAmount(SelectObjectData<Card> card, float coefficient = -1f) {
        float amount = card.obj.data.amount;
        if (card.owner.Equals(player)) amount = coefficient * amount;
        return amount;
    }
    private SelectObjectData<Card> FindBest(Func<SelectObjectData<Card>, float> countAmount) {
        var data = SelectManager.SelectData.selectingCards;
        SelectObjectData<Card> best = data[0];
        var max = countAmount(best);
        for (int i = 1; i < data.Count; i++) {
            var amount = countAmount(data[i]);
            if (amount > max) {
                max = amount;
                best = data[i];
            }
        }

        return best;
    }
}
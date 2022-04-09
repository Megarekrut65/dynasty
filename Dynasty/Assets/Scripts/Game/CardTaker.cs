public class CardTaker {
    private CardAnimationManager animationManager;
    private GameDependencies dependencies;
    private CardDependencies cardDependencies;
    private GameController gameController;

    public CardTaker(GameDependencies dependencies, CardDependencies cardDependencies) {
        this.animationManager = cardDependencies.CardAnimationManager;
        this.dependencies = dependencies;
        this.cardDependencies = cardDependencies;
        this.gameController = dependencies.gameController;
    }
    public Card TakeCardFromDesk() {
        if (gameController.GameOver|| dependencies.roundManager.Pause) return null;
        dependencies.roundManager.Pause = true;
        var card = cardDependencies.Table.TakeCardFromDesk();
        if (card.key == "inevitable-end") {
            gameController.GameOver = true;
        }
        cardDependencies.Controller.CreateCard(card);
        animationManager.PlayCardFromDeskAnimation(card.obj, () => {
            if (!gameController.GameOver)
                dependencies.bigCardManager.MakeBig(card.obj);
        });
        Player next = dependencies.roundManager.WhoIsNextPlayer();
        dependencies.logger.TranslatedLog($"{next.Nickname} took card \'{card.data.name}\' from desk");
        cardDependencies.Controller.AddClickToCard(card,
            cardDependencies.EffectsGenerator.GetEffect(dependencies.roundManager.GetTheNextPlayer(), card),
            next.GetColor(),
            dependencies.playerManager.IsPlayer(next));

        return card;
    }
    public bool PlayerRound() {
        return dependencies.playerManager.IsPlayer(dependencies.roundManager.WhoIsNextPlayer());
    }
}
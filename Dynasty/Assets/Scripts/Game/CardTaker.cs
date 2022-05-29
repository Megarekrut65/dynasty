public class CardTaker {
    private CardAnimationManager animationManager;
    private GameDependencies dependencies;
    private CardDependencies cardDependencies;
    private GameCloser gameCloser;

    public CardTaker(GameDependencies dependencies, CardDependencies cardDependencies) {
        this.animationManager = cardDependencies.CardAnimationManager;
        this.dependencies = dependencies;
        this.cardDependencies = cardDependencies;
        this.gameCloser = dependencies.gameCloser;
    }
    public Card TakeCardFromDesk() {
        if (gameCloser.GameOver || dependencies.roundManager.Pause) return null;
        dependencies.roundManager.Pause = true;
        var card = cardDependencies.Table.TakeCardFromDesk();
        if (card.key == "inevitable-end") {
            gameCloser.GameOver = true;
        }

        cardDependencies.Controller.CreateCard(card);
        animationManager.PlayCardFromDeskAnimation(card.obj, GameAction.Empty);
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
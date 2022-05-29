using UnityEngine;

public class CardDependencies {
    public Table Table { get; }
    public CardController Controller { get; }
    public IEffectsGenerator EffectsGenerator { get; }
    public AnimationEffectGenerator AnimationGenerator { get; }
    public CardAnimationManager CardAnimationManager { get; }
    public CardFullScreenMaker CardFullScreenMaker { get; }

    private GameDependencies dependencies;
    public CardDependencies(GameDependencies dependencies, GameObject container,
        GameObject cardObject, CardAnimationManager animationManager, CardFullScreenMaker cardFullScreenMaker) {
        this.dependencies = dependencies;
        this.CardAnimationManager = animationManager;
        this.CardFullScreenMaker = cardFullScreenMaker;
        Controller = new CardController(container, cardObject, cardFullScreenMaker);
        Table = new Table(dependencies.playerManager.Players);
        AnimationGenerator = new AnimationEffectGenerator(Controller, Table, animationManager);
        EffectsGenerator = new TakingEffectGenerator(dependencies, Controller, Table, AnimationGenerator, cardFullScreenMaker);
    }
    public void AddStartCards() {
        dependencies.playerManager.Players.ForEach(AddStartCards);
    }
    private void AddStartCards(Player player) {
        string avoidKey = "avoid-inevitable";
        CardData avoid = LocalizationManager.Instance.GetCard(avoidKey);
        Card card = new Card(avoid, avoidKey);
        Controller.CreateCard(card);
        Controller.AddClickToCard(card,
            EffectsGenerator.GetEffect(player, card), new Vector4(0f, 0f, 0f, 0f),
            dependencies.playerManager.IsPlayer(player));
        AnimationGenerator.AddCardToPlayerAnimated(card, player, () => { });
    }
}
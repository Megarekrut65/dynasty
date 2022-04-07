using UnityEngine;

public class CardDependencies {
    public Table Table{ get; }
    public CardController Controller { get; }
    public EffectsGenerator EffectsGenerator{ get; }
    public AnimationEffectGenerator AnimationGenerator{ get; }
    
    private GameDependencies dependencies;
    public CardDependencies(GameDependencies dependencies, GameObject container, GameObject cardObject, CardAnimationManager animationManager) {
        this.dependencies = dependencies;
        Controller = new CardController(container, cardObject);
        Table = new Table(dependencies.playerManager.Players);
        AnimationGenerator = new AnimationEffectGenerator(Controller, Table, animationManager);
        EffectsGenerator = new EffectsGenerator(dependencies, Controller, Table, AnimationGenerator);
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
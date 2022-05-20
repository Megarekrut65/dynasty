using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CardEffect = System.Func<bool>;
public class TakingEffectGenerator:SpecialEffectsGenerator {
    public TakingEffectGenerator(GameDependencies dependencies, CardController cardController, 
        Table table, AnimationEffectGenerator anim) : base(dependencies, cardController, table, anim) {
    }
    public override CardEffect GetEffect(Player player, Card card) {
        switch (card.key) {
            case "robin-hood":
            case "reliable-plan":
                return TakeFromEnemySelectEffect(player, card);
            case "wolf-spirit":
            case "magic-sphere":
                return MoveToOtherSelectEffect(player, card);
            case "dungeon-keys":
                return DungeonKeysEffect(player, card);
            case "castle":
            case "fairy-army":
            case "king-sword":
                return TakeAwayCardEffect("king", player, card);
            case "dragon":
                return MoveToCardEffect("gold-mountain", player, card);
            case "gold-mountain":
                return GoldMountainEffect(player, card);
            case "hydra":
                return HydraEffect(player, card);
            case "royal-rebellion":
                return RoyalRebellionEffect(player, card);
            case "encampment":
                return EncampmentEffect(player, card);
            case "tower":
                return TowerEffect(player, card);
            case "dracula":
                return DraculaEffect(player, card);
            case "look-back":
                return LookBackEffect(player, card);
            default:
                return base.GetEffect(player, card);
        }
    }
    private CardEffect LookBackEffect(Player player, Card card) {
        return () => {
            dependencies.cameraMove.Stop = true;
            dependencies.scrollManager.ViewSetActive(true);
            behaviour.StartCoroutine(TakeFromDrop(player));
            return CardEffect(player, card, false)();
        };
    }
    private IEnumerator TakeFromDrop(Player player) {
        var cards = table.GetRCardsInDrop();
        Action<bool> after = next => {
            CallNext(next)();
            dependencies.cameraMove.Stop = false;
            dependencies.scrollManager.ViewSetActive(false);
        };
        if (cards.Count != 0) {
            var card = cards[cards.Count - 1];
            table.RemoveCardFromDrop(card.id);
            cardController.CreateCard(card);
            dependencies.scrollManager.AddToScroll(card.obj);
            yield return new WaitForSeconds(2f);
            TakingCardEffect(player, card, after);
        } else after(true);
    }
    private void TakingCardEffect(Player player, Card card, Action<bool> end) {
        card.obj.GetComponent<CoverCard>().Uncover();
        CardEffect effect = GetEffect(player, card);
        if (!card.needSelect) {
            effect();
            end(false);
            return;
        }
        logger.LogGot(player, card);
        anim.AddCardToPlayerAnimated(card, player, ()=> {
            end(true);
        });
    }
    private CardEffect DungeonKeysEffect(Player player, Card card) {
        return () => MixEffect("secret-treasure", player, card, false)() &&
                     TakeAwayCardEffect("dungeon", player, card)();
    }
    protected CardEffect TakeAwayCardEffect(string key, Player player, Card card, bool callNext = true) {
        return () => {
            logger.LogAction(player, key, "took-away");
            var take = table.RemoveCardFromPlayer(key);
            if (take != null) {
                TakingCardEffect(player, take, next=>CardEffect(player, card, next)());
            } else CardEffectAction(callNext, player, card)();
            return true;
        };
    }
    protected CardEffect MoveToCardEffect(string key, Player player, Card card, bool callNext = true) {
        return () => {
            Player owner = player;
            Player with = table.FindPlayerWithCard(key);
            if (with != null) {
                owner = with;
                Card c = table.FindCardInPlayer(with, key);
                anim.PulsationCardAnimated(c);
            }
            return !callNext || CardEffect(owner, card)();
        };
    }
    private CardEffect GoldMountainEffect(Player player, Card card) {
        return () => DropEffect("gold-mountain", player, card, false)() &&
                     TakeAwayCardEffect("dragon", player, card)();
    }
    protected CardEffect TakeAwayCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
        Card card, bool callNext = true) {
        return () => {
            card.needSelect = true;
            selectManager.SelectMove(predicate, player, players,
                id => {
                    logger.LogAction(player, id, "took-away");
                    var take = table.RemoveCardFromPlayer(id);
                    if (take != null) {
                        TakingCardEffect(player, take, next=>CallNext(next)());
                    }
                    else CallNext(callNext)();
                }, dependencies.playerManager.IsPlayer(player));
            return CardEffect(player, card, false)();
        };
    }
    protected CardEffect MoveCardSelectEffect(Predicate<Card> predicate, Player player, List<Player> players,
        Card card, bool callNext = true) {
        return () => {
            card.needSelect = true;
            selectManager.SelectMoveToOther(predicate, player, players,
                (id, pl) => {
                    var take = table.RemoveCardFromPlayer(id);
                    if (take != null)
                        TakingCardEffect(pl, take, next=> {
                            logger.LogAction(player, id, "moved");
                            CallNext(next)();
                        });
                    else CallNext(callNext)();
                }, dependencies.playerManager.IsPlayer(player));
            return CardEffect(player, card, false)();
        };
    }
    private CardEffect DraculaEffect(Player player, Card card) {
        return MoveCardSelectEffect(c => "batcrow".Contains(c.key),
            player, dependencies.playerManager.Players, card);
    }
    private CardEffect TowerEffect(Player player, Card card) {
        return TakeAwayCardSelectEffect(c => c.key == "archer",
            player, dependencies.playerManager.GetEnemies(player), card);
    }
    private CardEffect EncampmentEffect(Player player, Card card) {
        return TakeAwayCardSelectEffect(c => c.type == CardType.KNIGHT,
            player, dependencies.playerManager.GetEnemies(player), card);
    }
    private CardEffect RoyalRebellionEffect(Player player, Card card) {
        return () => MixEffect("king", player, card, false)() &&
                     EncampmentEffect(player, card)();
    }
    protected CardEffect MoveToOtherSelectEffect(Player player, Card card) {
        return MoveCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
            player, dependencies.playerManager.Players, card);
    }
    protected CardEffect TakeFromEnemySelectEffect(Player player, Card card) {
        return TakeAwayCardSelectEffect(GameAction.GetAllFilter(CardFunctions.MOVE),
            player, dependencies.playerManager.GetEnemies(player), card);
    }
    private CardEffect HydraEffect(Player player, Card card) {
        return MoveCardSelectEffect(c => c.type == CardType.MONSTER && c.id != card.id,
            player, dependencies.playerManager.Players, card);
    }
}
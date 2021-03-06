using System.Collections.Generic;
using CardEffect = System.Func<bool>;

/// <summary>
/// Class that creates unusual cards effects
/// </summary>
public abstract class SpecialEffectsGenerator : SelectEffectGenerator {
    protected SpecialEffectsGenerator(GameDependencies dependencies,
        CardController cardController, Table table, AnimationEffectGenerator anim)
        : base(dependencies, cardController, table, anim) {
    }
    public override CardEffect GetEffect(Player player, Card card) {
        switch (card.key) {
            case "inevitable-end":
                return InevitableEndEffect(card);
            case "avoid-inevitable":
                return AvoidInevitableEffect(player, card);
            case "secret-treasure":
                return SecretTreasureEffect(player, card);
            case "swordsman":
            case "crusader":
            case "equestrian":
            case "archer":
                return KnightEffect(player, card);
            case "snake":
                return SnakeEffect(player, card);
            case "cerberus":
                return CerberusEffect(player, card);
            case "four-elements":
                return FourElementsEffect(player, card);
            case "royal-poison":
                return RoyalPoisonEffect(player, card);
            case "bear":
                return BearEffect(player, card);
            case "hunter":
                return HunterEffect(player, card);
            case "artillery":
                return ArtilleryEffect(player, card);
            case "boarding-party":
                return BoardingPartyEffect(player, card);
            case "killer":
                return KillerEffect(player, card);
            case "dungeon":
                return DungeonEffect(player, card);
            case "hut":
                return HutEffect(player, card);
            case "explosion":
                return ExplosionEffect(player, card);
            case "hocus-pocus":
                return HocusPocusEffect(player, card);
            default:
                return base.GetEffect(player, card);
        }
    }

    private CardEffect HocusPocusEffect(Player player, Card card) {
        return () => {
            table.NextFromDown = true;
            return CardMoreEffect(1, player, card)();
        };
    }
    private CardEffect ExplosionEffect(Player player, Card card) {
        return DropCardSelectEffect(GameAction.GetFilter(
                CardFunctions.DROP, c => c.underCard != null),
            player, TotemFilter(dependencies.playerManager.Players), card);
    }
    private CardEffect HutEffect(Player player, Card card) {
        return CoverCardSelectEffect(c => c.type == CardType.MONSTER,
            player, new List<Player>() {player}, card);
    }
    private CardEffect DungeonEffect(Player player, Card card) {
        return () => {
            BonusCoins("secret-treasure", 6, player.Nickname);
            return CoverPlayersCardSelectEffect(player, card)();
        };
    }
    private CardEffect KillerEffect(Player player, Card card) {
        return DropCardSelectEffect(c => c.type == CardType.KNIGHT,
            player, new List<Player>() {player}, card);
    }
    private CardEffect BoardingPartyEffect(Player player, Card card) {
        var siren = table.FindCardInPlayer(player, "siren");
        if (siren == null) {
            return DropOwnCardSelectEffect(player, card);
        }

        anim.PulsationCardAnimated(siren);
        return CardEffect(player, card);
    }
    private CardEffect ArtilleryEffect(Player player, Card card) {
        return () => {
            var castle = table.FindCardInPlayer(player, "black-sun-castle");
            if (castle == null) {
                return DropCardSelectEffect(c => c.type == CardType.BUILDING || c.type == CardType.WALL,
                    player, new List<Player>() {player}, card)();
            }

            anim.PulsationCardAnimated(castle);
            return CardEffect(player, card)();
        };
    }
    private CardEffect HunterEffect(Player player, Card card) {
        return () => {
            var dracula = table.FindCardInPlayer(player, "dracula");
            if (dracula == null) {
                return DropCardSelectEffect(c =>
                        c.type == CardType.MONSTER, player,
                    new List<Player>() {player}, card)();
            }

            anim.PulsationCardAnimated(dracula);
            return CardEffect(player, card)();
        };
    }
    private CardEffect BearEffect(Player player, Card card) {
        return MixCardSelectEffect(c => c.type == CardType.MONSTER,
            player, new List<Player>() {player}, card);
    }
    private CardEffect RoyalPoisonEffect(Player player, Card card) {
        return () => {
            var knights = table.RemoveAllCardsFromPlayers(null, c => c.type == CardType.KNIGHT);
            var king = table.RemoveCardFromPlayer("king");
            knights.Add(king);
            anim.MixAllAnimated(knights, CallNext());
            return CardEffect(player, card, false)();
        };
    }
    private CardEffect FourElementsEffect(Player player, Card card) {
        return () => {
            var elements = table.RemoveAllCardsFromPlayers(player, c => c.type == CardType.WALL);
            logger.LogSomeCards(player);
            anim.TakeAllAnimated(player, elements, CallNext());
            return CardEffect(player, card, false)();
        };
    }
    private CardEffect CerberusEffect(Player player, Card card) {
        return () => {
            var cerberus = table.FindAllCardsInPlayer(player, c => c.key == "cerberus");
            if (cerberus.Count >= 2) {
                foreach (var cer in cerberus) {
                    anim.PulsationCardAnimated(cer);
                }

                player.AddCoins(-2);
                logger.LogCoins(player, -2);
            }

            return CardEffect(player, card)();
        };
    }
    private CardEffect SnakeEffect(Player player, Card card) {
        return () => {
            Player snake = table.FindPlayerWithCard("snake");
            if (snake != null && snake.Nickname != player.Nickname) {
                var snakes = table.RemoveAllCardsFromPlayer(snake, c => c.key == "snake");
                logger.LogSomeCards(player);
                anim.TakeAllAnimated(player, snakes, CallNext());
                return CardEffect(player, card, false)();
            }

            return CardEffect(player, card)();
        };
    }
    private CardEffect KnightEffect(Player player, Card card) {
        return () => {
            Player owner = player;
            Player king = table.FindPlayerWithCard("king");
            if (king != null) {
                owner = king;
                var k = table.FindCardInPlayer(king, "king");
                anim.PulsationCardAnimated(k);
            }

            return CardEffect(owner, card)();
        };
    }
    private CardEffect SecretTreasureEffect(Player player, Card card) {
        return () => {
            BonusCoins("dungeon", 6, player.Nickname);
            return CardEffect(player, card)();
        };
    }

    private CardEffect InevitableEndEffect(Card card) {
        return () => {
            logger.GameOver();
            CoinsCounter coinsCounter = new CoinsCounter(dependencies, anim, logger);
            coinsCounter.StartCounting(table, () => { dependencies.gameCloser.DoGameOver(); });
            return false;
        };
    }
    private CardEffect AvoidInevitableEffect(Player player, Card card) {
        return () => {
            Player now = dependencies.roundManager.WhoIsNow() ?? dependencies.roundManager.WhoIsNextPlayer();
            if (!player.Equals(now)) return false;
            Card currentCard = table.Current;
            if (currentCard is {key: "inevitable-end"}) {
                table.Current = null;
                logger.LogAction(player, card.key, "dropped");
                anim.DropCardFromPlayerAnimated(card, player, () => {
                    logger.LogInsert(player, currentCard.data.name);
                    anim.InsertCardToDeskAnimated(currentCard, () => {
                        CallNext(true)();
                        dependencies.gameCloser.GameOver = false;
                    });
                });
                return true;
            }

            return false;
        };
    }
}
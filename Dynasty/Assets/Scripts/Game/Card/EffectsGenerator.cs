using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EffectsGenerator : SimpleEffectsGenerator
{
    public EffectsGenerator(GameManager gameManager, CardManager cardManager, Table table)
        : base(gameManager, cardManager, table) { }
    public Func<bool> GetEffect(Player player, Card card)
    {
        switch (card.key)
        {
            case "inevitable-end":
                return InevitableEndEffect(card);
            case "avoid-inevitable":
                return AvoidInevitableEffect(player, card);
            case "light-wizard":
                return MixEffect("dark-wizard", player, card);
            case "dark-wizard":
                return MixEffect("light-wizard", player, card);
            case "gold-mountain":
                return GoldMountainEffect(player, card);
            case "dragon":
                return MoveToCardEffect("gold-mountain", player, card);
            case "hero":
                return DropEffect("dragon", player, card);
            case "dungeon-keys":
                return DungeonKeysEffect(player, card);
            case "secret-treasure":
                return SecretTreasureEffect(player, card);
            case "castle":
            case "fairy-army":
            case "king-sword":
                return TakeAwayCardEffect("king", player, card);
            case "swordsman":
            case "crusader":
            case "equestrian":
            case "archer":
                return KnightEffect(player, card);
            case "step-ahead":
                return CardMoreEffect(1, player, card);
            case "snake":
                return SnakeEffect(player, card);
            default:
                return OtherEffect(player, card);
        }
    }
    private Func<bool> SnakeEffect(Player player, Card card)
    {
        return () =>
        {
            Player snake = table.GetPlayerWithCard("snake");
            if (snake != null && snake.nickname != player.nickname)
            {
                var snakes = table.GetAllCardFromPlayer(snake, c => c.key == "snake");
                gameManager.StartCoroutine(TakeAll(player, snakes));
                return OtherEffect(player, card, false)();
            }
            return OtherEffect(player, card)();
        };
    }
    IEnumerator TakeAll(Player player, List<Card> cards)
    {
        foreach (var card in cards)
        {
            player.AddCard(card);
            table.AddCardToPlayer(player, card);
            yield return new WaitForSeconds(0.1f);
        }
        gameManager.CallNext();
    }
    private Func<bool> KnightEffect(Player player, Card card)
    {
        return () =>
        {
            Player owner = player;
            Player king = table.GetPlayerWithCard("king");
            if (king != null)
            {
                owner = king;
            }
            return OtherEffect(owner, card)();
        };
    }
    private Func<bool> SecretTreasureEffect(Player player, Card card)
    {
        return () =>
        {
            Player dungeon = table.GetPlayerWithCard("dungeon");
            if (dungeon != null && dungeon.nickname == player.nickname)
            {
                player.AddCoins(6);
            }
            return OtherEffect(player, card)();
        };
    }
    private Func<bool> DungeonKeysEffect(Player player, Card card)
    {
        return () =>
        {
            return MixEffect("secret-treasure", player, card, false)() &&
                TakeAwayCardEffect("dungeon", player, card)();
        };
    }
    private Func<bool> GoldMountainEffect(Player player, Card card)
    {
        return () =>
        {
            return DropEffect("gold-mountain", player, card, false)() &&
                TakeAwayCardEffect("dragon", player, card)();
        };
    }
    IEnumerator CountCoins(KeyValuePair<Player, List<Card>> item)
    {
        var player = item.Key;
        var cards = item.Value;
        foreach (var card in cards)
        {
            int amount = card.data.amount;
            player.AddCoins(amount);
            MonoBehaviour.Destroy(card.obj);
            yield return new WaitForSeconds(0.5f);
        }
        cards.Clear();
    }
    private Func<bool> InevitableEndEffect(Card card)
    {
        return () =>
        {
            table.CountRCardCoins(item => { gameManager.StartCoroutine(CountCoins(item)); return true; });
            return false;
        };
    }
    private Func<bool> AvoidInevitableEffect(Player player, Card card)
    {
        return () =>
        {
            Card currentCard = table.Current;
            if (currentCard != null && currentCard.key == "inevitable-end")
            {
                cardManager.DeleteCardFromTable(currentCard);
                table.InsertToDesk(currentCard);
                cardManager.DeleteCardFromTable(card);
                table.DropCardFromPlayer(player, card);
                gameManager.GameOver = false;
                table.Current = null;
                return true;
            }
            return false;
        };
    }
}
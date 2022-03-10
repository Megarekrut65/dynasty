using System;
using UnityEngine;
using System.Collections.Generic;
public class TableManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cardObject;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private CardAnimationManager animationManager;
    [Header("Card places")]
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Desk[] playerDesks = new Desk[6];
    private Table table;
    private CardManager cardManager;
    private EffectsGenerator effectsGenerator;

    private List<CardBot> bots = new List<CardBot>();
    void Start()
    {
        cardManager = new CardManager(container, cardObject);
        gameManager.CreatePlayers();
        var players = gameManager.Players;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetDesk(playerDesks[i]);
        }
        table = new Table(players);
        effectsGenerator = new EffectsGenerator(gameManager, cardManager, table, animationManager);
        AddStartCards(players);
        int playerCount = players.Count - gameManager.BotCount;
        for (int i = playerCount; i < players.Count; i++)
        {
            bots.Add(new CardBot(players[i], gameManager, () => TakeCardFromDesk()));
        }
        gameManager.CallNext();
    }
    private void AddStartCards(List<Player> players)
    {
        string avoidKey = "avoid-inevitable";
        CardData avoid = LocalizationManager.instance.GetCard(avoidKey);
        foreach (var player in players)
        {
            Card card = new Card(avoid, avoidKey);
            cardManager.CreateCard(card);
            cardManager.AddClickToCard(card, effectsGenerator.GetEffect(player, card));
            animationManager.PlayCardHideShowAnimation(card.obj, () =>
            {
                player.AddCard(card);
                table.AddCardToPlayer(player, card);
                return true;
            }, () => { return true; });
        }
    }
    public bool PlayerRound()
    {
        for (int i = 0; i < gameManager.PlayerCount; i++)
        {
            if (gameManager.GetNextPlayer().nickname == gameManager.Players[i].nickname)
            {
                return true;
            }
        }
        return false;
    }
    public Card TakeCardFromDesk()
    {
        if (gameManager.GameOver || gameManager.Pause) return null;
        gameManager.Pause = true;
        var card = table.TakeCardFromDesk();
        cardManager.CreateCard(card);
        animationManager.PlayCardFromDesk(card.obj, () => { return true; });
        cardManager.AddClickToCard(card, () =>
        {
            bool res = effectsGenerator.GetEffect(gameManager.NextPlayer(), card)();
            return res;
        });
        if (card.key == "inevitable-end")
        {
            gameManager.GameOver = true;
        }
        return card;
    }
}
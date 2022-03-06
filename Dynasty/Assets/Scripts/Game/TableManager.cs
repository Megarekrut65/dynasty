using System;
using UnityEngine;
using System.Collections.Generic;
public class TableManager : MonoBehaviour {
    [SerializeField]
    private GameObject cardObject;
    [SerializeField]
    private GameManager gameManager;
    [Header("Card places")]
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Desk[] playerDesks = new Desk[6];
    private int playerCount = 6;
    private List<Player> players;
    private string[] nicknames = new string[6];
    private int index = 0;
    private Table table;
    private CardManager cardManager;
    private EffectsGenerator effectsGenerator;
    void Start(){
        cardManager = new CardManager(container, cardObject);
        players = new List<Player>();
        for(int i = 0; i < nicknames.Length; i++){
            nicknames[i] = "Player" + UnityEngine.Random.Range(0, 1000);
        }
        for(int i = 0; i < playerCount; i++){
            players.Add(new Player(nicknames[i], playerDesks[i]));
        }
        table = new Table(players);
        effectsGenerator = new EffectsGenerator(gameManager, cardManager, table);
        AddStartCards();
    }
    private void AddStartCards(){
        string avoidKey = "avoid-inevitable";
        CardData avoid = LocalizationManager.instance.GetCard(avoidKey);
        foreach(var player in players){
            Card card = new Card(avoid, avoidKey);
            cardManager.CreateCard(card);
            cardManager.AddClickToCard(card, effectsGenerator.GetEffect(player, card));
            player.AddCard(card);
            table.AddCardToPlayer(player, card);
        }
    }
    public void TakeCardFromDesk(){
        if(gameManager.GameOver) return;
        var card = table.TakeCardFromDesk();
        cardManager.CreateCard(card);
        cardManager.AddClickToCard(card, effectsGenerator.GetEffect(NextPlayer(), card));
        if(card.key == "inevitable-end"){
            gameManager.GameOver = true;
        }
    }
    
    private Player NextPlayer(){
        Player player = players[index++];
        if(index >= playerCount){
            index = 0;
        }
        return player;
    }
}
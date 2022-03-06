using System;
using UnityEngine;
using System.Collections.Generic;
public class TableManager : MonoBehaviour {
    private Table table;
    private ResizingData data = new ResizingData(1000);
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private GameObject cardObject;
    [SerializeField]
    private Desk[] playerDesks = new Desk[6];
    private string[] nicknames = new string[6];
    private int playerCount = 6;
    private List<Player> players;
    private int index = 0;
    private Stack<GameObject> cardPool = new Stack<GameObject>();
    void Start(){
        players = new List<Player>();
        for(int i = 0; i < nicknames.Length; i++){
            nicknames[i] = "Player" + UnityEngine.Random.Range(0, 1000);
        }
        for(int i = 0; i < playerCount; i++){
            players.Add(new Player(nicknames[i], playerDesks[i]));
        }
        table = new Table(players);
        AddStartCards();
    }
    private void AddStartCards(){
        string avoidKey = "avoid-inevitable";
        CardData avoid = LocalizationManager.instance.GetCard(avoidKey);
        foreach(var player in players){
            Card card = new Card(avoid, avoidKey);
            CreateCard(card);
            player.AddCard(card);
            table.AddCardToPlayer(player, card);
        }
    }
    public void TakeCardFromDesk(){
        var card = table.TakeCardFromDesk();
        CreateCard(card);
        //Use effect
        //after effect
        CardClick cardClick = card.obj.AddComponent<CardClick>() as CardClick;
        Func<bool> click = () =>{
            Player current = NextPlayer();
            current.AddCard(card);
            if(card.data.type == "A"){
                card.obj.SetActive(false);
                cardPool.Push(card.obj);
                card.obj = null;
                table.DropCard(current, card);
            }else table.AddCardToPlayer(current, card);
            Destroy(cardClick);
            return true;
        }; 
        cardClick.Click = click;
    }

    private Player NextPlayer(){
        Player player = players[index++];
        if(index >= playerCount){
            index = 0;
        }
        return player;
    }
    private void CreateCard(Card card){
        GameObject obj;
        if(cardPool.Count == 0)
            obj = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
        else{
            obj = cardPool.Pop();
            obj.SetActive(true);
        } 
        obj.GetComponent<LocalizationCard>().Key = card.key;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2 (305f/2, 495f/2);
        obj.transform.SetParent(container.transform, false);
        obj.GetComponent<LocalizationCard>().UpdateText();
        obj.GetComponent<ResizingTextCard>().Resize(data);
        card.obj = obj;
    }
}
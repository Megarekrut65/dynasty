using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EffectsGenerator {
        private GameManager gameManager;
        private CardManager cardManager;
        private Table table;
        
        public EffectsGenerator(GameManager gameManager, CardManager cardManager, Table table){
            this.gameManager = gameManager;
            this.cardManager = cardManager;
            this.table = table;
        }
        public Func<bool> GetEffect(Player player, Card card){
           switch (card.key) {
                case "inevitable-end":
                   return InevitableEndEffect(card);
                case "avoid-inevitable":
                   return AvoidInevitableEffect(player, card);
               default :
                   return OtherEffect(player, card);
           }
        }
        private Func<bool> OtherEffect(Player player, Card card){
            return () =>{
                //Use effect
                //after effect
                player.AddCard(card);
                if(card.data.type == "A"){
                    cardManager.DeleteCardFromTable(card);
                    table.DropCard(player, card);
                }else table.AddCardToPlayer(player, card);
                gameManager.CallNext();
                return true;
            };
        }
        IEnumerator CountCoins(KeyValuePair<Player, List<Card>> item){
            var player = item.Key;
            var cards = item.Value;
            foreach(var card in cards){
                int amount = card.data.amount;
                player.AddCoins(amount);
                MonoBehaviour.Destroy(card.obj);
                yield return new WaitForSeconds(0.5f);
            }
            cards.Clear();
        }
        private Func<bool> InevitableEndEffect(Card card){
            return () =>{
                table.CountRCardCoins(item =>{gameManager.StartCoroutine(CountCoins(item)); return true;});
                return false;
            };
        }
        private Func<bool> AvoidInevitableEffect(Player player, Card card){
            return () =>{
                Card currentCard = table.Current;
                if(currentCard != null && currentCard.key == "inevitable-end"){
                    cardManager.DeleteCardFromTable(currentCard);
                    table.InsertToDesk(currentCard);
                    cardManager.DeleteCardFromTable(card);
                    table.DropCard(player, card);
                    gameManager.GameOver = false;
                    table.Current = null;
                    return true;
                }
                return false;
            };
        }
    }   
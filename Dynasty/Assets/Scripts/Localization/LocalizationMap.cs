using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationMap{
    private SortedDictionary<string, string> wordMap{get; set;} = new SortedDictionary<string, string>();
    private SortedDictionary<string, CardData> cardMap{get; set;} = new SortedDictionary<string, CardData>();
    public SortedDictionary<string, string> WordMap{
        get{
            return wordMap;
        }
    }
    public SortedDictionary<string, CardData> CardMap{
        get{
            return cardMap;
        }
    }

   
    public void Change(SortedDictionary<string, string> wordMap, SortedDictionary<string, CardData> cardMap){
        this.wordMap = wordMap;
        this.cardMap = cardMap;
    }
    public string GetWord(string key){
        if(wordMap.ContainsKey(key)){
            return wordMap[key];
        }
        return default(string);
    }
    public CardData GetCard(string key){
        if(cardMap.ContainsKey(key)){
            return cardMap[key];
        }
        return default(CardData);
    }
    
}
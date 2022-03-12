using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationMap {
	private SortedDictionary<string, string> wordMap { get; set; } = new SortedDictionary<string, string>();
	private SortedDictionary<string, CardData> cardMap { get; set; } = new SortedDictionary<string, CardData>();
	public SortedDictionary<string, string> WordMap {
		get {
			return wordMap;
		}
	}
	public SortedDictionary<string, CardData> CardMap {
		get {
			return cardMap;
		}
	}

	public LocalizationMap(SortedDictionary<string, CardParameters> parameters) {
		foreach (var item in parameters) {
			cardMap.Add(item.Key, new CardData(item.Key, item.Value));
		}
	}
	public void Change(SortedDictionary<string, string> wordMap, SortedDictionary<string, CardInfo> cardMap) {
		this.wordMap = wordMap;
		foreach (var item in cardMap) {
			this.cardMap[item.Key].Change(item.Value);
		}
	}
	public string GetWord(string key) {
		if (wordMap.ContainsKey(key)) {
			return wordMap[key];
		}
		return default(string);
	}
	public CardData GetCard(string key) {
		if (cardMap.ContainsKey(key)) {
			return cardMap[key];
		}
		return default(CardData);
	}

}
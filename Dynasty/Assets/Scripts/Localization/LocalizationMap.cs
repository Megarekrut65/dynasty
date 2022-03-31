using System.Collections.Generic;

public class LocalizationMap {
	public SortedDictionary<string, string> WordMap { get; private set; } = new SortedDictionary<string, string>();

	public SortedDictionary<string, CardData> CardMap { get; } = new SortedDictionary<string, CardData>();

	public const string NOT_FOUND = "Error:Text not found";
	public LocalizationMap(SortedDictionary<string, CardParameters> parameters) {
		foreach (var item in parameters) {
			CardMap.Add(item.Key, new CardData(item.Key, item.Value));
		}
	}
	public void Change(SortedDictionary<string, string> wordMap, SortedDictionary<string, CardInfo> cardMap) {
		this.WordMap = wordMap;
		foreach (var item in cardMap) {
			this.CardMap[item.Key].Change(item.Value);
		}
	}
	public string GetWord(string key) {
		return WordMap.ContainsKey(key) ? WordMap[key] : NOT_FOUND;
	}
	public CardData GetCard(string key) {
		return CardMap.ContainsKey(key) ? CardMap[key] : default;
	}

}
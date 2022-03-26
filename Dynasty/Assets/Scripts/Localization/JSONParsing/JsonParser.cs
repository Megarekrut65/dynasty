using System.Collections.Generic;
using UnityEngine;

public class JsonParser<ValueType> {
	public static SortedDictionary<string, ValueType> Parse(string valuePath) {
		string path = Application.streamingAssetsPath + "/" + valuePath + ".json";
		string jsonData = AllFileReader.Read(path);
		LocalizationList<ValueType> list = JsonUtility.FromJson<LocalizationList<ValueType>>(jsonData);
		var map = new SortedDictionary<string, ValueType>();
		if (list.items != null) {
			for (int i = 0; i < list.items.Length; i++) {
				map.Add(list.items[i].key, list.items[i].value);
			}
		}
		return map;
	}
}
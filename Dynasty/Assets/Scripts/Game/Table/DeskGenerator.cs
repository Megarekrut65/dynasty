using System;
using System.Collections.Generic;

public static class DeskGenerator {
	public static List<Card> Generate(int seed, Func<Card, bool> check, int pos) {
		List<Card> desk = new List<Card>();
		List<Card> data = new List<Card>();
		var map = LocalizationManager.Instance.map.CardMap;
		List<Card> container = new List<Card>();//
		foreach (var item in map) {
			//UnityEngine.Debug.Log(item.Key);
			var card = new Card(item.Value, item.Key);
			if (check(card)) {
				for (int i = 0; i < item.Value.count; i++) container.Add(new Card(item.Value, item.Key));
				continue;
			}//
			for (int i = 0; i < item.Value.count; i++) {
				data.Add(new Card(item.Value, item.Key));
			}
		}
		System.Random rnd = new System.Random(seed);
		while (data.Count != 0) {
			int index = rnd.Next(data.Count);
			var item = data[index];
			desk.Add(item);
			data.Remove(item);
		}
		for (int i = 0; i < container.Count; i++) {
			//UnityEngine.Debug.Log(item.key);
			desk.Insert(desk.Count - pos, container[container.Count - 1 - i]);
		}//
		return desk;
	}
}
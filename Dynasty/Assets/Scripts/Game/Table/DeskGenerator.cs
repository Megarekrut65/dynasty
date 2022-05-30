using System;
using System.Collections.Generic;

/// <summary>
/// Class that creates all cards from card map
/// </summary>
public static class DeskGenerator {
    public static List<Card> Generate(Random rnd) {
        List<Card> desk = new List<Card>();
        List<Card> data = new List<Card>();
        var map = LocalizationManager.Instance.map.CardMap;
        foreach (var item in map) {
            for (int i = 0; i < item.Value.count; i++) {
                data.Add(new Card(item.Value, item.Key));
            }
        }

        while (data.Count != 0) {
            int index = rnd.Next(data.Count);
            var item = data[index];
            desk.Add(item);
            data.Remove(item);
        }

        return desk;
    }
}
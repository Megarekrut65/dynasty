using System;
using System.Collections.Generic;
using UnityEngine;

public class DeskGenerator
{
    public static List<Card> Generate(string[] toStart, int pos)
    {
        List<Card> desk = new List<Card>();
        List<Card> data = new List<Card>();
        var map = LocalizationManager.instance.map.CardMap;
        List<Card> container = new List<Card>();//
        foreach (var item in map)
        {
            if (Array.FindIndex(toStart, (i) => item.Key.Contains(i)) != -1)
            {
                for (int i = 0; i < item.Value.count; i++) container.Add(new Card(item.Value, item.Key));
                continue;
            }//
            for (int i = 0; i < item.Value.count; i++)
            {
                data.Add(new Card(item.Value, item.Key));
            }
        }
        System.Random rnd = new System.Random();
        while (data.Count != 0)
        {
            int index = rnd.Next(data.Count);
            var item = data[index];
            desk.Add(item);
            data.Remove(item);
        }
        foreach (var item in container)
        {
            desk.Insert(desk.Count - pos, item);
        }//
        return desk;
    }
}
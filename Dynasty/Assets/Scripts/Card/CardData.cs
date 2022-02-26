using System;
[System.Serializable]
public class CardData{
    public string name;
    public string description;
    public string move;
    public string mix;
    public int amount;
    public string cover;
    public string drop;
    public override string ToString()
    {
        return "name: {"+name + "}, description:{" + description + "}";
    }
}
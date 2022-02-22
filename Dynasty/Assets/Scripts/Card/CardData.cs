using System;
[System.Serializable]
public class CardData{
    public string name;
    public string description;
    public override string ToString()
    {
        return "name: {"+name + "}, description:{" + description + "}";
    }
}
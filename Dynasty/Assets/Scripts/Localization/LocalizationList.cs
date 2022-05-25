using System;

[Serializable]
public class LocalizationList<TValueType> {
    public ItemData<TValueType>[] items;
}

[Serializable]
public class ItemData<TValueType> {
    public string key;
    public TValueType value;
}
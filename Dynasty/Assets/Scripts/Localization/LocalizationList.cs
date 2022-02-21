using System.Collections;
using System.Collections.Generic;
using System.IO;
[System.Serializable]
public class LocalizationList<ValueType>{
    public ItemData<ValueType>[] items;
}
[System.Serializable]
public class ItemData<ValueType>{
    public string key;
    public ValueType value;

}
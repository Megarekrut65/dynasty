using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationChanger<ValueType>{
    private string currentLanguage;
    private string folder;
    private static string languageKey = "Language";
    public LocalizationChanger(string folder){
        this.folder = folder;
        map = new SortedDictionary<string, ValueType>();
    }
    private SortedDictionary<string, ValueType> map;
    public SortedDictionary<string, ValueType> GetLanguage(string language){
        if(language == currentLanguage) return map;
        currentLanguage = language;
        PlayerPrefs.SetString(languageKey, language);
        return GetValue(language);
    }
    public SortedDictionary<string, ValueType> GetValue(string valuePath){
        string path = Application.streamingAssetsPath +"/"+ folder + "/" + valuePath + ".json";
        string jsonData;
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(path);
            while (!reader.isDone) { }
 
            jsonData = reader.text;
        }
        else
        {
            jsonData = File.ReadAllText(path);
        }
        LocalizationList<ValueType> list = JsonUtility.FromJson<LocalizationList<ValueType>>(jsonData);
        map.Clear();
        if(list.items != null){
            for(int i = 0; i < list.items.Length; i++){
                map.Add(list.items[i].key, list.items[i].value);
            }
        }
        return map;
    }
}
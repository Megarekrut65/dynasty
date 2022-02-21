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
    }
    public void ChangeLanguage(string language){
        if(language == currentLanguage) return;
        currentLanguage = language;
        PlayerPrefs.SetString(languageKey, language);
        string path = Application.streamingAssetsPath + folder + "/" + language + ".json";
        string jsonData;
        // if (Application.platform == RuntimePlatform.Android)
        // {
        //     WWW reader = new WWW(path);
        //     while (!reader.isDone) { }
 
        //     jsonData = reader.text;
        // }
        // else
        // {
            
        // }
        jsonData = File.ReadAllText(path);
        LocalizationList<ValueType> list = JsonUtility.FromJson<LocalizationList<ValueType>>(jsonData);
        SortedDictionary<string, ValueType> map = new SortedDictionary<string, ValueType>();
        for(int i = 0; i < list.items.Length; i++){
            map.Add(list.items[i].key, list.items[i].value);
        }
        LocalizationMap<ValueType>.GetInstance().Change(map);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private string currentLanguage;
    private static string languageKey = "Language";
    private static bool isReady{get;set;} = false;

    public void ChangeLanguage(string language){
        if(language == currentLanguage) return;
        currentLanguage = language;
        PlayerPrefs.SetString(languageKey, language);
        string path = Application.streamingAssetsPath + "/Languages/" + language + ".json";
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
        LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonData);
        SortedDictionary<string, string> map = new SortedDictionary<string, string>();
        for(int i = 0; i < data.items.Length; i++){
            map.Add(data.items[i].key, data.items[i].value);
        }
        LocalizationMap.GetInstance().Change(map);
        isReady = true;
    }
     void Awake()
    {
        if (!PlayerPrefs.HasKey(languageKey))
        {
            if(Application.systemLanguage == SystemLanguage.Ukrainian)
            {
                PlayerPrefs.SetString(languageKey, "uk_UK");
            }
            else if (Application.systemLanguage == SystemLanguage.Russian ||
            Application.systemLanguage == SystemLanguage.Belarusian)
            {
                PlayerPrefs.SetString(languageKey, "ru_RU");
            }   
            else
            {
                PlayerPrefs.SetString(languageKey, "en_US");
            }
        }
        if(LocalizationMap.GetInstance().IsChanged)
        {
            currentLanguage = PlayerPrefs.GetString(languageKey);
        }else
        {
            currentLanguage = "";
            ChangeLanguage(PlayerPrefs.GetString(languageKey));
        }
    }
}

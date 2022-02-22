using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private static string languageKey = "Language";

    public void ChangeLanguage(string language){
        Change<string>( language, "Languages");
        Change<CardData>( language, "Languages/Cards");
    }
    private void Change<ValueType>(string language, string folder){
        LocalizationChanger<ValueType> changer = new LocalizationChanger<ValueType>(folder);
        changer.ChangeLanguage(language);
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
        if(!LocalizationMap<string>.GetInstance().IsChanged)
        {
            Change<string>(PlayerPrefs.GetString(languageKey), "Languages");
        }
        if(!LocalizationMap<CardData>.GetInstance().IsChanged)
        {
            Change<CardData>(PlayerPrefs.GetString(languageKey), "Languages/Cards");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private static string languageKey = "Language";

    public void ChangeLanguage(string language){
        LocalizationChanger<string> changer = new LocalizationChanger<string>("/Languages");
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
            ChangeLanguage(PlayerPrefs.GetString(languageKey));
        }
    }
}

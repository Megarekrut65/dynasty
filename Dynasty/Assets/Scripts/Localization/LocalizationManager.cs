using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private static string languageKey = "Language";
    private bool isReady = true;
    public bool Ready{
        get{
            return isReady;
        }
    }
    public static LocalizationManager instance;
    private LocalizationChanger<string> wordChanger = new LocalizationChanger<string>("Languages");
    private LocalizationChanger<CardData> cardChanger= new LocalizationChanger<CardData>("Languages/Cards");
    public LocalizationMap map = new LocalizationMap();
    public delegate void ChangeLanguageText();
    public event ChangeLanguageText OnLanguageChanged;
    public void ChangeLanguage(string language){
        if(!isReady) return;
        isReady = false;
        StartCoroutine( ChangeCoroutine(language));
    }
    IEnumerator ChangeCoroutine(string language){
        map.Change(wordChanger.GetLanguage(language), cardChanger.GetLanguage(language));
        yield return null;
        isReady = true;
        OnLanguageChanged?.Invoke();
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
        if (instance == null) {
            instance = this;
            ChangeLanguage(PlayerPrefs.GetString(languageKey));
        } else if (instance != this)
        {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }
    public string GetWord(string key){
        return map.GetWord(key);
    }
    public CardData GetCard(string key){
        return map.GetCard(key);
    }
}

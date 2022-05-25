using System.Collections;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {
    private const string LANGUAGE_KEY = "Language";
    private const string PARAMETERS_PATH = "Cards/parameters";
    private const string LANGUAGE_FOLDER = "Languages/";
    private const string CARD_FOLDER = "Languages/Cards/";

    public static LocalizationManager Instance { get; private set; }
    public bool Ready { get; private set; } = true;

    public LocalizationMap map;
    private string currentLanguage = "";

    public delegate void ChangeLanguageText();
    public event ChangeLanguageText OnLanguageChanged;

    public void ChangeLanguage(string language) {
        if (language == currentLanguage || !Ready) {
            OnLanguageChanged?.Invoke();
            return;
        }

        currentLanguage = language;
        PlayerPrefs.SetString(LANGUAGE_KEY, language);
        Ready = false;
        StartCoroutine(ChangeCoroutine(language));
    }

    private IEnumerator ChangeCoroutine(string language) {
        map.Change(JsonParser<string>.Parse(LANGUAGE_FOLDER + language),
            JsonParser<CardInfo>.Parse(CARD_FOLDER + language));
        yield return null;
        Ready = true;
        OnLanguageChanged?.Invoke();
    }
    private void SetLanguagePrefab() {
        if (!PlayerPrefs.HasKey(LANGUAGE_KEY)) {
            switch (Application.systemLanguage) {
                case SystemLanguage.Ukrainian:
                    PlayerPrefs.SetString(LANGUAGE_KEY, "uk_UK");
                    break;
                case SystemLanguage.German:
                    PlayerPrefs.SetString(LANGUAGE_KEY, "de_DE");
                    break;
                default:
                    PlayerPrefs.SetString(LANGUAGE_KEY, "en_US");
                    break;
            }
        }
    }

    private void Awake() {
        SetLanguagePrefab();
        if (Instance == null) {
            Instance = this;
            var parameters = JsonParser<CardParameters>.Parse(PARAMETERS_PATH);
            map = new LocalizationMap(parameters);
            ChangeLanguage(PlayerPrefs.GetString(LANGUAGE_KEY));
        } else if (Instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    public string GetWord(string key) {
        return map.GetWord(key);
    }
    public CardData GetCard(string key) {
        return map.GetCard(key);
    }
}
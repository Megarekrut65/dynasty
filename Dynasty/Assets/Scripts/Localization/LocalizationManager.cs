using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalizationManager : MonoBehaviour {
	private const string languageKey = "Language";
	private const string parametersPath = "Cards/parameters";
	private const string languageFolder = "Languages/";
	private const string cardFolder = "Languages/Cards/";

	private static LocalizationManager instance;
	public static LocalizationManager Instance {
		get {
			return instance;
		}
	}
	private bool isReady = true;
	public bool Ready {
		get {
			return isReady;
		}
	}
	public LocalizationMap map;
	private string currentLanguage = "";

	public delegate void ChangeLanguageText();
	public event ChangeLanguageText OnLanguageChanged;

	public void ChangeLanguage(string language) {
		if (language == currentLanguage || !isReady) return;
		currentLanguage = language;
		PlayerPrefs.SetString(languageKey, language);
		isReady = false;
		StartCoroutine(ChangeCoroutine(language));
	}
	IEnumerator ChangeCoroutine(string language) {
		map.Change(JsonParser<string>.Parse(languageFolder + language),
					JsonParser<CardInfo>.Parse(cardFolder + language));
		yield return null;
		isReady = true;
		OnLanguageChanged?.Invoke();
	}
	private void SetLanguagePrefab() {
		if (!PlayerPrefs.HasKey(languageKey)) {
			if (Application.systemLanguage == SystemLanguage.Ukrainian) {
				PlayerPrefs.SetString(languageKey, "uk_UK");
			} else if (Application.systemLanguage == SystemLanguage.German) {
				PlayerPrefs.SetString(languageKey, "de_DE");
			} else {
				PlayerPrefs.SetString(languageKey, "en_US");
			}
		}
	}
	void Awake() {
		SetLanguagePrefab();
		if (instance == null) {
			instance = this;
			var parameters = JsonParser<CardParameters>.Parse(parametersPath);
			map = new LocalizationMap(parameters);
			ChangeLanguage(PlayerPrefs.GetString(languageKey));
		} else if (instance != this) {
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

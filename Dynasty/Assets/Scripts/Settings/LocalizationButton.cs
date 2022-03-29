using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LocalizationButton : MonoBehaviour,
IPointerDownHandler, IPointerUpHandler {
	[SerializeField]
	private string language = "";
	[SerializeField]
	private LanguageLoader languageLoader;

	public void OnPointerDown(PointerEventData eventData) {
		languageLoader.SetActive(true);
		LocalizationManager.Instance.ChangeLanguage(language);
	}
	public void OnPointerUp(PointerEventData eventData) {

	}
	private void OnDestroy() {
		LocalizationManager.Instance.OnLanguageChanged -= ChangeLanguage;
	}
	private void Awake() {
		LocalizationManager.Instance.OnLanguageChanged += ChangeLanguage;
	}
	private void Start() {
		ChangeLanguage();
	}
	protected void ChangeLanguage() {
		var color = new Color(0f, 0f, 0f, 0f);
		if (PlayerPrefs.HasKey("Language") &&
			PlayerPrefs.GetString("Language").Equals(language)) {
			color = new Color(0f, 0f, 0f, 1f);
			StartCoroutine(Hide());
		}
		GetComponent<Outline>().effectColor = color;
	}
	private IEnumerator Hide() {
		yield return new WaitForSeconds(0.5f);
		languageLoader.SetActive(false);
	}
}

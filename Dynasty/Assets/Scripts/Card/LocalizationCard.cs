using System.Diagnostics;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocalizationCard : MonoBehaviour
{
    [SerializeField]
    private string key;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text descriptionText;
    public string Key{
        set{
            key = value;
        }
    }
 
    void Awake()
    {
        LocalizationMap<CardData>.GetInstance().OnLanguageChanged += UpdateText;
    }
 
    void Start()
    {
        UpdateText();
    }
 
    private void OnDestroy()
    {
        LocalizationMap<CardData>.GetInstance().OnLanguageChanged -= UpdateText;
    }
 
    virtual protected void UpdateText()
    {
        if (nameText == null || descriptionText == null) return;
        CardData card = LocalizationMap<CardData>.GetInstance().GetValue(key);
        if(card == null) return;
        nameText.text = card.name;
        descriptionText.text = card.description.Replace('#', '\n');
    }
}

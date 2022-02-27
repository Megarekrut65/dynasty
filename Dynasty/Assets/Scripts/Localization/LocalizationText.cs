using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocalizationText : MonoBehaviour
{
    [SerializeField]
    private string key;
 
    private Text text;
 
    void Awake()
    {
        if(text == null)
        {
            text = GetComponent<Text>();
        }
        LocalizationManager.instance.OnLanguageChanged += UpdateText;
    }
 
    void Start()
    {
        UpdateText();
    }
 
    void OnDestroy()
    {
        LocalizationManager.instance.OnLanguageChanged -= UpdateText;
    }
 
    virtual protected void UpdateText()
    {
        if (gameObject == null) return;
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        text.text = LocalizationManager.instance.GetWord(key);
    }
}

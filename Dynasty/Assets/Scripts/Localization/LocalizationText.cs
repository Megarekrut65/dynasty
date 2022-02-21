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
        LocalizationMap<string>.GetInstance().OnLanguageChanged += UpdateText;
    }
 
    void Start()
    {
        UpdateText();
    }
 
    private void OnDestroy()
    {
        LocalizationMap<string>.GetInstance().OnLanguageChanged -= UpdateText;
    }
 
    virtual protected void UpdateText()
    {
        if (gameObject == null) return;
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        text.text = LocalizationMap<string>.GetInstance().GetValue(key);
    }
}

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
        LocalizationMap.GetInstance().OnLanguageChanged += UpdateText;
    }
 
    void Start()
    {
        UpdateText();
    }
 
    private void OnDestroy()
    {
        LocalizationMap.GetInstance().OnLanguageChanged -= UpdateText;
    }
 
    virtual protected void UpdateText()
    {
        if (gameObject == null) return;
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        text.text = LocalizationMap.GetInstance().GetValue(key);
    }
}

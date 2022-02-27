using System;
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
    [SerializeField]
    private Image[] icons = new Image[4];
    [SerializeField]
    private Image amountImage;
    [SerializeField]
    private Text amountText;
    public string Key{
        set{
            key = value;
        }
    }
    private CardData card;
    void Awake()
    {
        //LocalizationMap<CardData>.GetInstance().OnLanguageChanged += UpdateText;
    }
 
    void Start()
    {
        //UpdateText();
    }
 
    private void OnDestroy()
    {
        //LocalizationMap<CardData>.GetInstance().OnLanguageChanged -= UpdateText;
    }
    private void SetIcon(Image icon, string value){
        if(icon == null) return;
       switch (value) {
           case "+":
           case "yes":{
               icon.color = new Color(0, 255, 0);
           }
               break;
            case"-":
            case "no":{
                icon.color = new Color(255, 0, 0);
           }
               break;  
            case "none":{
                icon.color = new Color(0, 0, 0, 0);
           }
               break; 
            case "0":{
                 icon.color = new Color(244, 205, 0);
            } 
            break;
           default :
               
               break;
       }
    }
    private void SetIcons(){
        SetIcon(icons[0], card.move);
        SetIcon(icons[1], card.mix);
        SetIcon(icons[2], card.cover);
        SetIcon(icons[3], card.drop);
    }
    private void SetText(){
        nameText.text = card.name;
        descriptionText.text = card.description.Replace('#', '\n');
    }
    private void SetAmount(){
        int amount = card.amount;
        int sign = Math.Sign(amount);
        string text = "";
       switch (sign) {
            case 0:{
                SetIcon(amountImage, "0");
           }
               break;
            case 1:{
                text = "+";
                SetIcon(amountImage, "+");
           }
               break;
            case -1:{
                SetIcon(amountImage, "-");
           }
               break;
           default :
               
               break;
       }
        amountText.text = text + amount.ToString();
    }
    virtual protected void UpdateText()
    {
        if (nameText == null || descriptionText == null) return;
        card = LocalizationMap<CardData>.GetInstance().GetValue(key);
        if(card == null) return;
        SetText();
        SetAmount();
        SetIcons();
    }
}

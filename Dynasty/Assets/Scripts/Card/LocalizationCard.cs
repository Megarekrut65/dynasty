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
        public string Key{
        set{
            key = value;
        }
    }
    private CardData card;
    public CardData Card{
        get{
            return card;
        }
    }
    [Header("Sprites Data")]
    [SerializeField]
    private Sprite[] sprites = new Sprite[4];
    [SerializeField]
    private Sprite[] types = new Sprite[2];
    [Header("Card parts")]
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

 
    private void SetColor(Image icon, string value){
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
    // private void SetIconSprites(){
    //     SetSprite(card.move, 0);
    //     SetSprite(card.mix, 1);
    //     SetSprite(card.cover, 2);
    //     SetSprite(card.drop, 3);
    // }
    // private void SetSprite(string value, int index){
    //     if(value != "none"){
    //         icons[index].sprite = sprites[index];
    //     }
    // }
    private void SetIcons(){
        SetIconColors();
        //SetIconSprites();
    }
    private void SetIconColors(){
        SetColor(icons[0], card.move);
        SetColor(icons[1], card.mix);
        SetColor(icons[2], card.cover);
        SetColor(icons[3], card.drop);
    }
    private void SetText(){
        nameText.text = card.name;
        descriptionText.text = card.description.Replace('#', '\n');
    }
    private void SetType(){
        amountImage.sprite = card.type == "R"?types[0]:types[1];
    }
    private void SetAmount(){
        int amount = card.amount;
        int sign = Math.Sign(amount);
        string text = "";
       switch (sign) {
            case 0:{
                SetColor(amountImage, "0");
           }
               break;
            case 1:{
                text = "+";
                SetColor(amountImage, "+");
           }
               break;
            case -1:{
                SetColor(amountImage, "-");
           }
               break;
           default :
               
               break;
       }
        amountText.text = text + amount.ToString();
        SetType();
    }
    public void UpdateText()
    {
        if (nameText == null || descriptionText == null) return;
        card = LocalizationManager.instance.GetCard(key);
        if(card == null) return;
        SetText();
        SetAmount();
        SetIcons();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
public class CardManager
{
    private GameObject container;
    private GameObject cardObject;
    private ResizingData data = new ResizingData(1000);
    private Stack<GameObject> cardPool = new Stack<GameObject>();

    public CardManager(GameObject container, GameObject cardObject)
    {
        this.container = container;
        this.cardObject = cardObject;
    }
    public void DeleteCardFromTable(Card card)
    {
        CardClick cardClick = card.obj.GetComponent<CardClick>();
        ButtonScript buttonScript = card.obj.GetComponent<ButtonScript>();
        if (cardClick != null) MonoBehaviour.Destroy(cardClick);
        if (buttonScript != null) MonoBehaviour.Destroy(buttonScript);
        //card.obj.SetActive(false);
        //cardPool.Push(card.obj);
        MonoBehaviour.Destroy(card.obj);
        card.obj = null;
    }
    public void AddClickToCard(Card card, Func<bool> func)
    {
        CardClick cardClick = card.obj.AddComponent<CardClick>() as CardClick;
        ButtonScript buttonScript = card.obj.AddComponent<ButtonScript>() as ButtonScript;
        Func<bool> click = () =>
        {
            bool res = func();
            if (res)
            {
                MonoBehaviour.Destroy(cardClick);
                MonoBehaviour.Destroy(buttonScript);
            }
            return res;
        };
        cardClick.Click = click;
    }
    public void CreateCard(Card card)
    {
        GameObject obj;
        // if (cardPool.Count == 0)
        obj = MonoBehaviour.Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
        // else
        // {
        //     obj = cardPool.Pop();
        //     obj.SetActive(true);
        // }
        obj.GetComponent<LocalizationCard>().Key = card.key;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(305f / 4, 495f / 4);
        // obj.transform.localScale = new Vector3(1f, 1f, 1f);
        // obj.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
        // obj.transform.position = new Vector3(0f, 0f, 0f);
        obj.transform.SetParent(container.transform, false);
        obj.GetComponent<LocalizationCard>().UpdateText();
        obj.GetComponent<ResizingTextCard>().Resize(data);
        card.obj = obj;
    }
}
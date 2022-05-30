using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardEffect = System.Func<bool>;

/// <summary>
/// Class that creates and deletes cards
/// </summary>
public class CardController {
    private GameObject container;
    private GameObject cardObject;
    private Stack<GameObject> cardPool = new Stack<GameObject>();

    public CardController(GameObject container, GameObject cardObject) {
        this.container = container;
        this.cardObject = cardObject;
    }
    public void DeleteCardFromTable(Card card) {
        CardClick cardClick = card.obj.GetComponent<CardClick>();
        if (cardClick != null) Object.Destroy(cardClick);
        Object.Destroy(card.obj);
        card.obj = null;
    }
    public void AddClickToCard(Card card, CardEffect effect, Color color, bool canClick) {
        CardClick cardClick = card.obj.AddComponent<CardClick>() as CardClick;
        var outline = CreateOutline(card.obj, color);
        CardEffect click = () => {
            bool res = effect();
            if (res) {
                Object.Destroy(cardClick);
                Object.Destroy(outline);
            }

            return res;
        };
        cardClick.Click = click;
        cardClick.Key = card.key;
        cardClick.CanClick = canClick;
    }
    public static Outline CreateOutline(GameObject obj, Color color) {
        Outline outline = obj.AddComponent<Outline>();
        if (color.a > 0f) color.a = 0.5f;
        outline.effectColor = color;
        outline.effectDistance = new Vector2(7f, 7f);
        return outline;
    }
    public void CreateCard(Card card) {
        GameObject obj;
        if (cardPool.Count == 0)
            obj = Object.Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
        else {
            obj = cardPool.Pop();
            obj.SetActive(true);
        }

        var rect = obj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(305f / 4, 495f / 4);
        obj.transform.SetParent(container.transform, false);
        card.obj = obj;
        UpdateCardObject(card);
    }
    private void UpdateCardObject(Card card) {
        var obj = card.obj;
        obj.GetComponent<CardLoader>().Key = card.key;
        obj.GetComponent<CardLoader>().LoadData();
        obj.GetComponent<ResizingTextCard>().BestFit(true);
    }
    public void CoverCard(Card under, Card top) {
        top.obj.GetComponent<CoverCard>().Cover();
        under.obj.GetComponent<CoverCard>().AddToContainer(top.obj);
    }
}
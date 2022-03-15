using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardManager {
	private GameObject container;
	private GameObject cardObject;
	private ResizingData data = new ResizingData(1000);
	private Stack<GameObject> cardPool = new Stack<GameObject>();

	public CardManager(GameObject container, GameObject cardObject) {
		this.container = container;
		this.cardObject = cardObject;
	}
	public void DeleteCardFromTable(Card card) {
		CardClick cardClick = card.obj.GetComponent<CardClick>();
		if (cardClick != null) MonoBehaviour.Destroy(cardClick);
		//card.obj.SetActive(false);
		//cardPool.Push(card.obj);
		MonoBehaviour.Destroy(card.obj);
		card.obj = null;
	}
	public void AddClickToCard(Card card, Func<bool> func, Color color, bool canClick) {
		CardClick cardClick = card.obj.AddComponent<CardClick>() as CardClick;
		var outline = CreateOutline(card.obj, color);
		Func<bool> click = () => {
			bool res = func();
			if (res) {
				MonoBehaviour.Destroy(cardClick);
				MonoBehaviour.Destroy(outline);
			}
			return res;
		};
		cardClick.Click = click;
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
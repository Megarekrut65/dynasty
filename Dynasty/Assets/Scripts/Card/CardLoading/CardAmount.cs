using System;
using UnityEngine;
using UnityEngine.UI;

public class CardAmount : MonoBehaviour {
	[SerializeField]
	private Sprite[] amountTypes = new Sprite[2];//0 - hexagon, 1 - pentagon
	[SerializeField]
	private Image amountImage;
	[SerializeField]
	private Text amountText;

	public void LoadAmount(CardData card) {
		int amount = card.amount;
		int sign = Math.Sign(amount);
		string text = "";
		switch (sign) {
			case 0: {
					ColorEditor.SetColor(amountImage, "0");
				}
				break;
			case 1: {
					text = "+";
					ColorEditor.SetColor(amountImage, "+");
				}
				break;
			case -1: {
					ColorEditor.SetColor(amountImage, "-");
				}
				break;
		}
		amountText.text = text + amount.ToString();
		LoadType(card);
	}
	private void LoadType(CardData card) {
		amountImage.sprite = card.type == "R" ? amountTypes[0] : amountTypes[1];
	}
}
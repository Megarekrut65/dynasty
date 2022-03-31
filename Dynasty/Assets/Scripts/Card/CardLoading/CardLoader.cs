using UnityEngine;

public class CardLoader : MonoBehaviour {
	[Header("Card Data")]
	[SerializeField]
	private string key;
	public string Key {
		set => key = value;
	}
	public CardData Card => LocalizationManager.Instance.GetCard(key);
	[Header("Card parts")]
	[SerializeField]
	private CardImage cardImage;
	[SerializeField]
	private CardText cardText;
	[SerializeField]
	private CardIcons cardIcons;
	[SerializeField]
	private CardAmount cardAmount;

	public void LoadData() {
		cardImage.LoadImage(Card);
		cardText.LoadText(Card);
		cardIcons.LoadIcons(Card);
		cardAmount.LoadAmount(Card);
	}
}

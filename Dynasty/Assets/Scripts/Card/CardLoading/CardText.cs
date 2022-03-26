using UnityEngine;
using UnityEngine.UI;

public class CardText : MonoBehaviour {
	[SerializeField]
	private Text nameText;
	[SerializeField]
	private Text descriptionText;

	public void LoadText(CardData card) {
		nameText.text = card.name;
		descriptionText.text = card.description.Replace('#', '\n');
	}
}
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Desk : MonoBehaviour {
	[Header("Desk parts")]
	[SerializeField]
	private Text nameLabel;
	[SerializeField]
	private Text coins;
	[SerializeField]
	private GameObject container;
	[SerializeField]
	private Color color;
	public Color PlayerColor {
		get {
			return color;
		}
	}
	void Start() {
		coins.text = "0";
		nameLabel.color = color;
	}
	public void AddCard(Card card) {
		card.obj.transform.SetParent(container.transform, false);
	}
	public void SetName(string name) {
		nameLabel.text = name;
	}
	public void SetCoins(int coins) {
		this.coins.text = coins.ToString();
	}
}
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Desk : MonoBehaviour {
	[SerializeField]
	private int order = 0;
	[Header("Desk parts")]
	[SerializeField]
	private GameObject playerLabel;
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
	public GameObject PlayerLabel {
		get {
			return playerLabel;
		}
	}
	public int Order {
		get {
			return order;
		}
	}

	private void Start() {
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
	public void SetActive(bool value) {
		gameObject.SetActive(value);
		playerLabel.SetActive(value);
	}
}
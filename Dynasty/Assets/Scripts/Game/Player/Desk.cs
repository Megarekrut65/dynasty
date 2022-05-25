using UnityEngine;
using UnityEngine.UI;

public class Desk : MonoBehaviour {
    [SerializeField]
    private int order = 0;
    [Header("Desk parts")]
    [SerializeField]
    private GameObject border;
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
    public Color PlayerColor => color;
    public GameObject PlayerLabel => playerLabel;
    public int Order => order;
    public string Name => nameLabel.text;

    private void Start() {
        coins.text = "0";
        nameLabel.color = color;
    }
    public void AddCard(Card card) {
        card.obj.transform.SetParent(container.transform, false);
    }
    public void SetName(string playerName) {
        nameLabel.text = playerName;
    }
    public void SetCoins(int playerCoins) {
        this.coins.text = playerCoins.ToString();
    }
    public void SetActive(bool value) {
        gameObject.SetActive(value);
        playerLabel.SetActive(value);
        border.SetActive(value);
    }
}
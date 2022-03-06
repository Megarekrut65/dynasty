using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Desk : MonoBehaviour {
    [SerializeField]
    private Text nameLabel;
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Text coins;

    void Start(){
        coins.text = "0";
    }
    public void AddCard(Card card){
        card.obj.transform.SetParent(container.transform, false);
    }
    public void SetName(string name){
        nameLabel.text = name;
    }
    public void SetCoins(int coins){
        this.coins.text = coins.ToString();
    }
}
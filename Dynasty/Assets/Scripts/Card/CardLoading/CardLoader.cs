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
    private CardImageLoader cardImageLoader;
    [SerializeField]
    private CardTextLoader cardTextLoader;
    [SerializeField]
    private CardIconsLoader cardIconsLoader;
    [SerializeField]
    private CardAmountLoader cardAmountLoader;

    public void LoadData() {
        cardImageLoader.LoadImage(Card);
        cardTextLoader.LoadText(Card);
        cardIconsLoader.LoadIcons(Card);
        cardAmountLoader.LoadAmount(Card);
    }
}
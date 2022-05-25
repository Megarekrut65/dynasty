using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour {
    [SerializeField]
    private Image image;
    public void LoadImage(CardData card) {
        var sprite = Resources.Load("Cards/Images/"
                                    + LocalStorage.GetValue(LocalStorage.IMAGE_FOLDER, "FromGoogle") + "/"
                                    + card.key, typeof(Sprite)) as Sprite;
        if (sprite != null) {
            image.sprite = sprite;
        }
    }
}
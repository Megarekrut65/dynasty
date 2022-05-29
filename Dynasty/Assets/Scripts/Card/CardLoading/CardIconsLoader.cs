using UnityEngine;
using UnityEngine.UI;

public class CardIconsLoader : MonoBehaviour {
    [SerializeField]
    private Image[] icons = new Image[4];

    public void LoadIcons(CardData card) {
        ColorEditor.SetColor(icons[0], card.move);
        ColorEditor.SetColor(icons[1], card.mix);
        ColorEditor.SetColor(icons[2], card.cover);
        ColorEditor.SetColor(icons[3], card.drop);
    }
}
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that add card to big place that make card big
/// </summary>
public class BigCardManager {
    public bool NeedMakeBig { get; set; }
    private GameObject cardPlace;

    public BigCardManager(GameObject cardPlace, Toggle toggle) {
        this.cardPlace = cardPlace;
        NeedMakeBig = toggle.isOn;
        toggle.onValueChanged.AddListener(Change);
    }
    public void MakeBig(GameObject obj) {
        if (NeedMakeBig) obj.transform.SetParent(cardPlace.transform, false);
    }
    private void Change(bool value) {
        NeedMakeBig = value;
    }
}
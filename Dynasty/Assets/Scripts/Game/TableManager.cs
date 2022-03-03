using UnityEngine;
using System.Collections.Generic;
public class TableManager : MonoBehaviour {
    private Table table;
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private GameObject cardObject;
    private ResizingData data = new ResizingData(1000);
    void Start(){
        table = new Table(new List<Player>());
    }
    public void TakeCardFromDesk(){
        var card = table.TakeCardFromDesk();
        GameObject obj = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
        obj.GetComponent<LocalizationCard>().Key = card.key;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2 (305f/2, 495f/2);
        obj.transform.SetParent(container.transform, false);
        obj.GetComponent<LocalizationCard>().UpdateText();
        obj.GetComponent<ResizingTextCard>().Resize(data);
    }
}
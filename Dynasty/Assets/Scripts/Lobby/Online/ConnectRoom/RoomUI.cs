using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour {
    [SerializeField]
    private GameObject clickObject;
    public GameObject ClickObject => clickObject;
    [SerializeField]
    private Text textName;
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private Image image;

    public void LoadData(string roomName, RoomInfo roomInfo) {
        name = roomName;
        textName.text = roomName;
        UpdateData(roomInfo);
    }
    public void UpdateData(RoomInfo roomInfo) {
        if (textCount != null) textCount.text = $"{roomInfo.currentCount}/{roomInfo.playerCount}";
    }
    public void UpdateColor(Color color) {
        image.color = color;
    }
}
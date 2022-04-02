using UnityEngine;
using UnityEngine.UI;

public class RoomObject : MonoBehaviour {
    [SerializeField]
    private Text textName;
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private Image image;

    public void LoadData(string roomName, RoomInfo roomInfo) {
        name = roomName;
        textName.text = roomName;
        textCount.text = $"{roomInfo.currentCount}/{roomInfo.playerCount}";
    }
    public void UpdateColor(Color color) {
        image.color = color;
    }
}
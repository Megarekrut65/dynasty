using UnityEngine;

public class RoomClick : MonoBehaviour {
    public ConnectToRoom Connect { get; set; }

    public void Click() {
        Connect.Click();
    }
}
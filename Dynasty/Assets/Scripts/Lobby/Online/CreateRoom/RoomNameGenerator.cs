using UnityEngine;
using Random = UnityEngine.Random;

public class RoomNameGenerator{
    public static string Generate() {
        string roomName = "";
        if (PlayerPrefs.HasKey(PrefabsKeys.PLAYER_NAME)) {
            roomName += PlayerPrefs.GetString(PrefabsKeys.PLAYER_NAME);
        }
        roomName += Random.Range(0, 10000);
        
        return roomName;
    }
}
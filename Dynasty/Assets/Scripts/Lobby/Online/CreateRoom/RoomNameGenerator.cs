using UnityEngine;
using Random = UnityEngine.Random;

public static class RoomNameGenerator{
    public static string Generate() {
        string roomName = "";
        if (PlayerPrefs.HasKey(LocalStorage.PLAYER_NAME)) {
            roomName += PlayerPrefs.GetString(LocalStorage.PLAYER_NAME);
        }
        roomName += Random.Range(0, 10000);
        
        return roomName;
    }
}
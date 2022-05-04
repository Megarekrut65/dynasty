using Firebase.Database;

public static class DatabaseReferences {
    public static DatabaseReference GetRoomReference() {
        return FirebaseDatabase
            .DefaultInstance
            .RootReference
            .Child(LocalStorage.ROOMS)
            .Child(LocalStorage.GetValue(LocalStorage.ROOM_NAME, "Room"));
    }
    public static DatabaseReference GetPlayerReference() {
        return GetRoomReference()
            .Child(GameKeys.PLAYERS)
            .Child(LocalStorage.GetValue(LocalStorage.PLAYER_KEY, "1"));
    }
}
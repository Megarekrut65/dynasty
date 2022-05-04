using UnityEngine;

public static class LocalStorage {
    public const string PLAYER_COUNT = "playerCount";
    public const string BOT_COUNT = "botCount";
    public const string ENABLE_BOTS = "enableBots";
    public const string DIFFICULTY_BOTS = "difficultyBots";
    public const string KEEP_PRIVATE = "keepPrivate";
    public const string PLAYER_NAME = "playerName";
    public const string GAME_MODE = "gameMode";
    public const string ROOM_NAME = "roomName";
    public const string ROOM_INFO = "roomInfo";
    public const string ROOMS = "rooms";
    public const string IS_HOST = "isHost";
    public const string BIG_CARD = "bigCard";
    public const string HIDE_CHAT = "hideChat";
    public const string HIDE_ROOM_INFO = "hideRoomInfo";
    public const string PLAYER_KEY = "playerKey";
    public const string DESK_SEED = "deskSeed";

    public static string GetValue(string key, string def = "") {
        if (!PlayerPrefs.HasKey(key)) {
            PlayerPrefs.SetString(key, def);
        }

        return PlayerPrefs.GetString(key);
    }
    public static int GetValue(string key, int def) {
        if (!PlayerPrefs.HasKey(key)) {
            PlayerPrefs.SetInt(key, def);
        }

        return PlayerPrefs.GetInt(key);
    }
}
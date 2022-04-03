using UnityEngine;

public static class PrefabsKeys {
    public const string PLAYER_COUNT = "player-count";
    public const string BOT_COUNT = "bot-count";
    public const string ENABLE_BOTS = "enable-bots";
    public const string DIFFICULTY_BOTS = "difficulty-bots";
    public const string KEEP_PRIVATE = "keep-private";
    public const string PLAYER_NAME = "player-name";
    public const string GAME_MODE = "game-mode";
    public const string ROOM_NAME = "room-name";
    public const string ROOM_INFO = "room-info";
    public const string ROOMS = "rooms";
    public const string IS_HOST = "is-host";
    public const string BIG_CARD = "big-card";
    public const string HIDE_CHAT = "hide-chat";
    public const string HIDE_ROOM_INFO = "hide-room-info";

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
public static class GameModeFunctions {
    public static bool IsMode(GameMode mode) {
        return LocalStorage.GetValue(LocalStorage.GAME_MODE, GameMode.OFFLINE.ToString()) == mode.ToString();
    }
}
public static class GameModeFunctions {
    public static bool IsMode(GameMode mode) {
        return PrefabsKeys.GetValue(PrefabsKeys.GAME_MODE, GameMode.OFFLINE.ToString()) == mode.ToString();
    }
}
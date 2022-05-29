using System;
using Firebase.Database;

public static class EntityControllerFactory {
    public static EntityController CreateController(string name, Player player,
        GameDependencies dependencies, Table table,CardFullScreenMaker cardFullScreenMaker, Func<Card> takeCard) {
        return name switch {
            MEDIUM => new MoreCoinsBotController(player, dependencies, table,cardFullScreenMaker, takeCard),
            ONLINE => new OnlineEntityController(player, dependencies, table,cardFullScreenMaker, takeCard,
                FirebaseDatabase.DefaultInstance.RootReference.Child(LocalStorage.ROOMS)
                    .Child(LocalStorage.GetValue(LocalStorage.ROOM_NAME)).Child(GameKeys.PLAYERS).Child(player.Key)),
            _ => new RandomBotController(player, dependencies, table,cardFullScreenMaker, takeCard) //easy
        };
    }
    public const string ONLINE = "online";
    public const string MEDIUM = "medium";
}
using System;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

/// <summary>
/// Implementation of GameController for online mode
/// </summary>
public class OnlineGameController : GameController {
    private string roomName;
    private DatabaseReference roomReference;
    private RoomUI roomUI;
    private RoomInfo roomInfo;
    private Desk[] desks;

    public OnlineGameController(GameDependencies gameDependencies,
        CardDependencies cardDependencies, CardTaker cardTaker, Desk[] desks, RoomUI roomUI) :
        base(gameDependencies, cardDependencies, cardTaker) {
        this.desks = desks;
        this.roomUI = roomUI;
        gameDependencies.logger.TranslatedLog("wait for players");
        roomName = LocalStorage.GetValue(LocalStorage.ROOM_NAME, "Room");
        roomReference = DatabaseReferences.GetRoomReference();
        roomReference.Child(LocalStorage.ROOM_INFO).ValueChanged += RoomChanged;
        roomReference.Child(GameKeys.PLAYERS).ValueChanged += PlayersChanged;
        GameCloser.GameOverEvent += GameOverEvent;
    }
    private void Unsubscribe() {
        roomReference.Child(LocalStorage.ROOM_INFO).ValueChanged -= RoomChanged;
        roomReference.Child(GameKeys.PLAYERS).ValueChanged -= PlayersChanged;
        GameCloser.GameOverEvent -= GameOverEvent;
    }
    private void GameOverEvent() {
        Unsubscribe();
        if (gameDependencies.gameStarter.GameStarted) {
            resultCreator.MakeResult();
            OpenScene("GameOver");
            return;
        }

        OpenScene("Lobby");
    }
    private void Start() {
        LoadPlayers();
        StartGame();
    }
    public override void Leave() {
        roomInfo.currentCount--;
        roomUI.LoadData(roomName, roomInfo);
        if (roomInfo.currentCount == 0) {
            roomReference.RemoveValueAsync().ContinueWithOnMainThread(task => GameOverEvent());
            return;
        }

        roomReference.Child(LocalStorage.ROOM_INFO).Child(GameKeys.CURRENT_COUNT).SetValueAsync(roomInfo.currentCount)
            .ContinueWithOnMainThread(task => {
                roomReference.Child(GameKeys.PLAYERS).Child(LocalStorage.GetValue(LocalStorage.PLAYER_KEY, "0"))
                    .RemoveValueAsync().ContinueWithOnMainThread(t => GameOverEvent());
            });
    }
    private void LoadPlayers() {
        int currentPlayerKey = Convert.ToInt32(LocalStorage.GetValue(LocalStorage.PLAYER_KEY, "1"));
        for (int i = 0; i < roomInfo.playerCount; i++) {
            string playerName = desks[i].Name;
            if ((i + 1) != currentPlayerKey) {
                var player = gameDependencies.playerManager.AddController(playerName, gameDependencies,
                    cardDependencies.Table,
                    cardTaker.TakeCardFromDesk);
                cardDependencies.Table.AddPlayer(player);
                continue;
            }

            Player current = new Player(playerName, desks[i],
                currentPlayerKey.ToString());
            gameDependencies.playerManager.Players.Add(current);
            ((OnlinePlayerManager) gameDependencies.playerManager).Current = current;
            cardDependencies.Table.AddPlayer(current);
        }
    }
    private void RoomChanged(object sender, ValueChangedEventArgs args) {
        roomInfo = JsonUtility.FromJson<RoomInfo>(args.Snapshot.GetRawJsonValue());
        if (roomInfo == null) return;
        roomUI.LoadData(roomName, roomInfo);
        if (gameDependencies.gameStarter.GameStarted && roomInfo.currentCount == 1) {
            Unsubscribe();
            resultCreator.MakeWinResult();
            roomReference.RemoveValueAsync().ContinueWithOnMainThread(task => OpenScene("GameOver"));
            return;
        }

        if (roomInfo.currentCount == roomInfo.playerCount) Start();
    }
    private void PlayersChanged(object sender, ValueChangedEventArgs args) {
        for (int i = 0; i < desks.Length; i++) {
            var snapshot = args.Snapshot.Child((i + 1).ToString()).Child(LocalStorage.PLAYER_NAME);
            desks[i].SetName(snapshot.Value == null ? "" : snapshot.Value as string);
        }

        if (gameDependencies.gameStarter.GameStarted) {
            var players = gameDependencies.playerManager.Players;
            foreach (var player in players) {
                int key = Convert.ToInt32(player.Key) - 1;
                player.Nickname = desks[key].Name;
            }
        }
    }
}
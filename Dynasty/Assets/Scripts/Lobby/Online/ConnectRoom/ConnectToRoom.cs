using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToRoom {
    private DatabaseReference roomReference;
    private string roomName;
    private LoadBoard loadBoard;
    private bool isClicked = false;
    
    public ConnectToRoom(DatabaseReference roomReference, string roomName, LoadBoard loadBoard) {
        this.roomReference = roomReference;
        this.roomName = roomName;
        this.loadBoard = loadBoard;
    }
    public void Click() {
        if (isClicked) return;
        isClicked = true;
        loadBoard.SetActive(true);
        roomReference.RunTransaction(data => {
            Dictionary<string, object> roomMap = (Dictionary<string, object>) data.Child(LocalStorage.ROOM_INFO).Value;
            RoomInfo roomInfo = new RoomInfo {
                currentCount = Convert.ToInt32(roomMap[GameKeys.CURRENT_COUNT]),
                playerCount = Convert.ToInt32(roomMap[LocalStorage.PLAYER_COUNT])
            };
            if (roomInfo.currentCount >= roomInfo.playerCount) return TransactionResult.Abort();
            roomMap[GameKeys.CURRENT_COUNT] = roomInfo.currentCount + 1;
            data.Child(LocalStorage.ROOM_INFO).Value = roomMap;
            
            string playerKey = PlayerDatabaseUtilities
                .GetPlayerKey(key => data.Child(GameKeys.PLAYERS).Child(key).Value);
            if(Convert.ToInt32(playerKey) > roomInfo.playerCount) return TransactionResult.Abort();
            data.Child(GameKeys.PLAYERS).Child(playerKey).Child(LocalStorage.PLAYER_NAME).Value =
                LocalStorage.GetValue(LocalStorage.PLAYER_NAME);
            PlayerPrefs.SetString(LocalStorage.PLAYER_KEY, playerKey);
            return TransactionResult.Success(data);
        }).ContinueWithOnMainThread(SetRoomInfoToPlayerPrefs)
            .ContinueWithOnMainThread(task => { loadBoard.SetActive(false); });
    }
    private void SetRoomInfoToPlayerPrefs(Task<DataSnapshot> task) {
        if (task.Exception != null) {
            Debug.LogError(task.Exception);
            return;
        }
        var roomInfo = JsonUtility.FromJson<RoomInfo>(task.Result.Child(LocalStorage.ROOM_INFO).GetRawJsonValue());
        if (roomInfo == null) return;
        PlayerPrefs.SetString(LocalStorage.ROOM_NAME, roomName);
        PlayerPrefs.SetInt(LocalStorage.PLAYER_COUNT, roomInfo.playerCount);
        PlayerPrefs.SetInt(LocalStorage.DESK_SEED, roomInfo.deskSeed);
        PlayerPrefs.SetString(LocalStorage.IS_HOST, false.ToString());
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
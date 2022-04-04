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
        if(isClicked) return;
        isClicked = true;
        loadBoard.SetActive(true);
        roomReference.Child(PrefabsKeys.ROOM_INFO).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                Debug.LogError(task.Exception);
                return;
            }
            var roomInfo = JsonUtility.FromJson<RoomInfo>(task.Result.GetRawJsonValue());
            if(roomInfo == null || roomInfo.currentCount == roomInfo.playerCount) return;
            PlayerPrefs.SetString(PrefabsKeys.ROOM_NAME, roomName);
            PlayerPrefs.SetInt(PrefabsKeys.PLAYER_COUNT, roomInfo.playerCount);
            PlayerPrefs.SetString(PrefabsKeys.IS_HOST, false.ToString());
            ConnectPlayerToGame.Connect(roomReference, PrefabsKeys.GetValue(PrefabsKeys.PLAYER_NAME, "player"),
                task2 => {
                    if (task2.Exception != null) {
                        Debug.LogError(task2.Exception);
                        return;
                    }
                    roomReference.Child(PrefabsKeys.ROOM_INFO).Child("currentCount")
                        .SetValueAsync(roomInfo.currentCount + 1).ContinueWithOnMainThread(
                            task3 => {
                                if (task3.Exception != null) {
                                    Debug.LogError(task3.Exception);
                                    return;
                                }
                                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                            });
                });
        }).ContinueWithOnMainThread(task => {
            loadBoard.SetActive(false);
        });
    }
}

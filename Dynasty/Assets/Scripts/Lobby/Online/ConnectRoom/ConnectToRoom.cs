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
        if(isClicked) return;
        isClicked = true;
        loadBoard.SetActive(true);
        roomReference.Child(PrefabsKeys.ROOM_INFO).GetValueAsync().
            ContinueWithOnMainThread(SetRoomInfoToPlayerPrefs).
            ContinueWithOnMainThread(task => { loadBoard.SetActive(false); });
    }
    private void SetRoomInfoToPlayerPrefs(Task<DataSnapshot> task) {
        if (task.Exception != null) {
            Debug.LogError(task.Exception);
            return;
        }
        var roomInfo = JsonUtility.FromJson<RoomInfo>(task.Result.GetRawJsonValue());
        if(roomInfo == null || roomInfo.currentCount == roomInfo.playerCount) return;
        PlayerPrefs.SetString(PrefabsKeys.ROOM_NAME, roomName);
        PlayerPrefs.SetInt(PrefabsKeys.PLAYER_COUNT, roomInfo.playerCount);
        PlayerPrefs.SetInt(PrefabsKeys.DESK_SEED, roomInfo.deskSeed);
        PlayerPrefs.SetString(PrefabsKeys.IS_HOST, false.ToString());
        PrintAboutPlayerInDatabase.Print(roomReference, PrefabsKeys.GetValue(PrefabsKeys.PLAYER_NAME, "player"),
            task2 => IncreasePlayerCount(task2, roomInfo));
    }
    private void IncreasePlayerCount(Task task, RoomInfo roomInfo) {
        if (task.Exception != null) {
            Debug.LogError(task.Exception);
            return;
        }
        roomReference.Child(PrefabsKeys.ROOM_INFO).Child("currentCount")
            .SetValueAsync(roomInfo.currentCount + 1).ContinueWithOnMainThread(LoadGameScene);
    }
    private void LoadGameScene(Task task) {
        if (task.Exception != null) {
            Debug.LogError(task.Exception);
            return;
        }
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}

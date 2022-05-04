using System;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateRoom : MonoBehaviour {
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject loadBoardObject;
    private LoadBoard loadBoard;

    private void Start() {
        loadBoard = new LoadBoard(loadBoardObject, canvas);
    }
    public void Create() {
        loadBoard.SetActive(true);
        var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(LocalStorage.ROOMS);
        var roomName = RoomNameGenerator.Generate();
        PlayerPrefs.SetString(LocalStorage.ROOM_NAME, roomName);
        RoomInfo roomInfo = new RoomInfo(PlayerPrefs.HasKey(LocalStorage.PLAYER_COUNT)
                ? PlayerPrefs.GetInt(LocalStorage.PLAYER_COUNT)
                : 2, 1,
            DateTime.UtcNow.ToString(),
            PlayerPrefs.HasKey(LocalStorage.KEEP_PRIVATE) &&
            Convert.ToBoolean(
                PlayerPrefs.GetString(LocalStorage
                    .KEEP_PRIVATE)), PlayerPrefs.GetInt(LocalStorage.DESK_SEED, 0));
        reference = reference.Child(roomName);
        reference.Child(LocalStorage.ROOM_INFO).SetRawJsonValueAsync(JsonUtility.ToJson(roomInfo)).ContinueWithOnMainThread(task => {
            PrintAboutPlayerInDatabase.Print(reference, LocalStorage.GetValue(LocalStorage.PLAYER_NAME), Created);
        });
    }
    private void Created(Task task) {
        if (task.Exception != null) {
            Debug.LogError(task.Exception);
            return;
        }
        loadBoard.SetActive(false);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
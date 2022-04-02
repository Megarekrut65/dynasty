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
        var reference = FirebaseDatabase.DefaultInstance.RootReference.Child(PrefabsKeys.ROOMS);
        var roomName = RoomNameGenerator.Generate();
        PlayerPrefs.SetString(PrefabsKeys.ROOM_NAME, roomName);
        PlayerPrefs.SetInt(PrefabsKeys.BOT_COUNT, 0);
        RoomInfo roomInfo = new RoomInfo(PlayerPrefs.HasKey(PrefabsKeys.PLAYER_COUNT)
                ? PlayerPrefs.GetInt(PrefabsKeys.PLAYER_COUNT)
                : 2, 1,
            DateTime.UtcNow.ToString(),
            PlayerPrefs.HasKey(PrefabsKeys.KEEP_PRIVATE) &&
            Convert.ToBoolean(
                PlayerPrefs.GetString(PrefabsKeys
                    .KEEP_PRIVATE)));
        reference = reference.Child(roomName).Child(PrefabsKeys.ROOM_INFO);
        reference.SetRawJsonValueAsync(JsonUtility.ToJson(roomInfo)).ContinueWithOnMainThread(Created);
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
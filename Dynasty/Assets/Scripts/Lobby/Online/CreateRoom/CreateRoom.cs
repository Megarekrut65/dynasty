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
    private int count = 0;
    private readonly object @lock = new object();
    
    private void Start() {
        loadBoard = new LoadBoard(loadBoardObject, canvas);
    }
    public void Create() {
        loadBoard.SetActive(true);
        count = 0;
        var reference = FirebaseDatabase.DefaultInstance.RootReference.Child("room-list");
        var roomName = RoomNameGenerator.Generate();
        PlayerPrefs.SetString(PrefabsKeys.ROOM_NAME, roomName);
        PlayerPrefs.SetInt(PrefabsKeys.BOT_COUNT, 0);
        reference = reference.Child(roomName);
        reference.Child(PrefabsKeys.KEEP_PRIVATE).SetValueAsync(PlayerPrefs.HasKey(PrefabsKeys.KEEP_PRIVATE) &&
                                                                Convert.ToBoolean(
                                                                    PlayerPrefs.GetString(PrefabsKeys.KEEP_PRIVATE)))
            .ContinueWithOnMainThread(Created);
        reference.Child(PrefabsKeys.PLAYER_COUNT).SetValueAsync(PlayerPrefs.HasKey(PrefabsKeys.PLAYER_COUNT)
            ? PlayerPrefs.GetInt(PrefabsKeys.PLAYER_COUNT)
            : 2).ContinueWithOnMainThread(Created);
        reference.Child("date").SetValueAsync(DateTime.UtcNow.ToString("hh:mm:ss MM-dd-yyyy"))
            .ContinueWithOnMainThread(Created);
    }
    private void Created(Task task) {
        if (task.Exception != null) {
            Debug.LogError(task.Exception);
            return;
        }
        lock (@lock) {
            count++;
            if (count == 3) {
                Debug.Log("Go");
                loadBoard.SetActive(false);
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
        }
    }
}
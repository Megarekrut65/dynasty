using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[SerializeField]
	private RoomObject roomObject;
	[SerializeField]
	private DependenciesManager dependenciesManager = new DependenciesManager();
	public bool GameOver { get; set; } = false;
	public GameDependencies Dependencies => dependenciesManager.GetDependencies();
	public GameMode Mode { get; private set; }
	public bool IsHost { get; private set; }
	public DatabaseReference RoomReference { get; private set; }
	public RoomInfo RoomInfo { get; private set; }
	public string roomName;
	
	private void Start() {
		Mode = PrefabsKeys.GetValue(PrefabsKeys.GAME_MODE, "offline") == "offline"
			? GameMode.OFFLINE
			: GameMode.ONLINE;
		if (Mode == GameMode.ONLINE) {
			IsHost = Convert.ToBoolean(PrefabsKeys.GetValue(PrefabsKeys.IS_HOST, false.ToString()));
			roomName = PrefabsKeys.GetValue(PrefabsKeys.ROOM_NAME, "Room");
			RoomReference = FirebaseDatabase.DefaultInstance.RootReference.Child(PrefabsKeys.ROOMS)
				.Child(roomName);
			RoomReference.GetValueAsync().ContinueWithOnMainThread(task => {
				if (task.Exception != null) {
					Debug.LogError(task.Exception);
				}
				RoomInfo = JsonUtility.FromJson<RoomInfo>(task.Result.Child(PrefabsKeys.ROOM_INFO).GetRawJsonValue());
				roomObject.LoadData(roomName, RoomInfo);
			});
		}
	}
	public void Leave() {
		if (Mode == GameMode.OFFLINE) {
			OpenScene(null);
			return;
		}
		RoomInfo.currentCount--;
		roomObject.LoadData(roomName, RoomInfo);
		if (RoomInfo.currentCount == 0) {
			RoomReference.RemoveValueAsync().ContinueWithOnMainThread(OpenScene);
		} else {
			RoomReference.Child(PrefabsKeys.ROOM_INFO).Child("current-count").SetValueAsync(RoomInfo.currentCount)
				.ContinueWithOnMainThread(OpenScene);
		}
	}
	private void OpenScene(Task task) {
		SceneManager.LoadScene("Menu", LoadSceneMode.Single);
	}
	private void OnApplicationQuit() {
		Leave();
	}
}
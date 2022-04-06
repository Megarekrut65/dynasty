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
		Mode = GameModeFunctions.IsMode(GameMode.OFFLINE)
			? GameMode.OFFLINE
			: GameMode.ONLINE;
		if (Mode == GameMode.ONLINE) {
			Connect();
		}
	}
	private void Connect() {
		IsHost = Convert.ToBoolean(PrefabsKeys.GetValue(PrefabsKeys.IS_HOST, false.ToString()));
		roomName = PrefabsKeys.GetValue(PrefabsKeys.ROOM_NAME, "Room");
		RoomReference = FirebaseDatabase.DefaultInstance.RootReference.Child(PrefabsKeys.ROOMS)
			.Child(roomName);
		RoomReference.Child(PrefabsKeys.ROOM_INFO).ValueChanged += (sender, args) => {
			RoomInfo = JsonUtility.FromJson<RoomInfo>(args.Snapshot.GetRawJsonValue());
			roomObject.LoadData(roomName, RoomInfo);
		};
		RoomReference.Child("players").ValueChanged += (sender, args) => {
			var desks = dependenciesManager.PlayerDesks;
			for (int i = 0; i < desks.Length; i++) {
				var snapshot = args.Snapshot.Child((i + 1).ToString());
				desks[i].SetName(snapshot.Value==null?"":snapshot.Value as string);
			}
		};
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
			RoomReference.Child(PrefabsKeys.ROOM_INFO).Child("currentCount").SetValueAsync(RoomInfo.currentCount)
				.ContinueWithOnMainThread(task => {
					RoomReference.Child("players").Child(PrefabsKeys.GetValue(PrefabsKeys.PLAYER_KEY, "0"))
						.RemoveValueAsync().ContinueWithOnMainThread(OpenScene);
				});
		}
	}
	private void OpenScene(Task task) {
		SceneManager.LoadScene("Menu", LoadSceneMode.Single);
	}
}
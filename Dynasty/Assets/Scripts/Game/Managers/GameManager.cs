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
	
	public GameDependencies GameDependencies => dependenciesManager.GetGameDependencies();
	public CardDependencies CardDependencies => dependenciesManager.GetCardDependencies();
	private GameMode mode;
	public bool IsHost { get; private set; }
	public DatabaseReference RoomReference { get; private set; }
	public RoomInfo RoomInfo { get; private set; }
	public string roomName;

	private void Start() {
		mode = GameModeFunctions.IsMode(GameMode.OFFLINE)
			? GameMode.OFFLINE
			: GameMode.ONLINE;
		if (mode == GameMode.ONLINE) {
			GameDependencies.logger.TranslatedLog("wait for players");
			Connect();
		}
	}
	public void StartGame() {
		CardDependencies.AddStartCards();
		GameDependencies.gameController.StartGame();
	}
	private void Connect() {
		IsHost = Convert.ToBoolean(PrefabsKeys.GetValue(PrefabsKeys.IS_HOST, false.ToString()));
		roomName = PrefabsKeys.GetValue(PrefabsKeys.ROOM_NAME, "Room");
		RoomReference = FirebaseDatabase.DefaultInstance.RootReference.Child(PrefabsKeys.ROOMS).Child(roomName);
		RoomReference.Child(PrefabsKeys.ROOM_INFO).ValueChanged += RoomChanged;
		RoomReference.Child("players").ValueChanged += PlayersChanged;
	}
	private void RoomChanged(object sender, ValueChangedEventArgs args) {
		RoomInfo = JsonUtility.FromJson<RoomInfo>(args.Snapshot.GetRawJsonValue());
		if(RoomInfo == null) return;
		roomObject.LoadData(roomName, RoomInfo);
		if(RoomInfo.currentCount == RoomInfo.playerCount) StartGame();
	}
	private void PlayersChanged(object sender, ValueChangedEventArgs args) {
		var desks = dependenciesManager.PlayerDesks;
		for (int i = 0; i < desks.Length; i++) {
			var snapshot = args.Snapshot.Child((i + 1).ToString());
			desks[i].SetName(snapshot.Value==null?"":snapshot.Value as string);
		}
	}
	public void Leave() {
		if (mode == GameMode.OFFLINE) {
			OpenScene(null);
			return;
		}
		RoomInfo.currentCount--;
		roomObject.LoadData(roomName, RoomInfo);
		if (RoomInfo.currentCount == 0) {
			RoomReference.RemoveValueAsync().ContinueWithOnMainThread(OpenScene);
			return;
		}
		RoomReference.Child(PrefabsKeys.ROOM_INFO).Child("currentCount").SetValueAsync(RoomInfo.currentCount)
			.ContinueWithOnMainThread(task => {
				RoomReference.Child("players").Child(PrefabsKeys.GetValue(PrefabsKeys.PLAYER_KEY, "0"))
					.RemoveValueAsync().ContinueWithOnMainThread(OpenScene);
			});
	}
	private void OpenScene(Task task) {
		SceneManager.LoadScene("Menu", LoadSceneMode.Single);
	}
}
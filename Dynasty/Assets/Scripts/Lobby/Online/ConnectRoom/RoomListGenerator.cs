using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class RoomListGenerator : MonoBehaviour {
    [SerializeField]
    private GameObject roomObject;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private Color[] colors = new Color[2];
    private List<GameObject> rooms = new List<GameObject>();
    private DatabaseReference roomReference;
    private Stack<GameObject> roomsPool = new Stack<GameObject>();
    private void Start() {
        roomReference = FirebaseDatabase.DefaultInstance.RootReference.Child(PrefabsKeys.ROOMS);
        roomReference.ValueChanged += ChangeRooms;
        // roomReference.GetValueAsync().ContinueWithOnMainThread(task => {
        //     if (task.Exception != null) {
        //         Debug.LogError(task.Exception);
        //         return;
        //     }
        //     AddRoomsToList(task.Result.Children);
        // });
    }
    private void OnEnable() {
        
        
    }
    private void OnDisable() {
        //FirebaseDatabase.DefaultInstance.RootReference.Child(PrefabsKeys.ROOMS).ValueChanged -= ChangeRooms;
    }
    private void ChangeRooms(object sender, ValueChangedEventArgs args ) {
       AddRoomsToList(args.Snapshot.Children);
    }
    private void AddRoomsToList(IEnumerable<DataSnapshot> data) {
        foreach (var child in data) {
            string roomName = child.Key;
            var roomInfoJson = child.Child(PrefabsKeys.ROOM_INFO).GetRawJsonValue();
            var roomInfo = JsonUtility.FromJson<RoomInfo>(roomInfoJson);
            if(roomInfo == null) continue;
            var obj = rooms.Find(room => room.name == roomName);
            if (obj != null && roomInfo.keepPrivate || roomInfo.currentCount >= roomInfo.playerCount)
                RemoveRoom(obj);
            else if (obj == null)rooms.Add(CreateRoom(roomName, roomInfo));
        }
        UpdateList();
    }
    private GameObject CreateRoom(string roomName, RoomInfo roomInfo) {
        GameObject obj = null;
        if (roomsPool.Count != 0) {
            obj = roomsPool.Pop();
            obj.SetActive(true);
            obj.transform.SetParent(content, false);
        }
        else obj = Instantiate(roomObject, content);
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 30f);
        obj.GetComponent<RoomObject>().LoadData(roomName, roomInfo);
        return obj;
    }
    private void RemoveRoom(GameObject obj) {
        rooms.Remove(obj);
        roomsPool.Push(obj);
        obj.SetActive(false);
    }
    private void UpdateList() {
        for (int i = 0; i < rooms.Count; i++) {
            rooms[i].GetComponent<RoomObject>().UpdateColor(colors[i%2]);
        }
    }
}

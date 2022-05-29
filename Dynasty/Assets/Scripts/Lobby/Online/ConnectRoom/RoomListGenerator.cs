using System;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class RoomListGenerator : MonoBehaviour {
    [Header("Load board")]
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject board;
    private LoadBoard loadBoard;
    [Header("Rooms")]
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
        loadBoard = new LoadBoard(board, canvas);
        roomReference = FirebaseDatabase.DefaultInstance.RootReference.Child(LocalStorage.ROOMS);
        roomReference.ValueChanged += ChangeRooms;
    }
    private void OnDestroy() {
        roomReference.ValueChanged -= ChangeRooms;
    }
    private void ChangeRooms(object sender, ValueChangedEventArgs args) {
        foreach (var child in args.Snapshot.Children) {
            string roomName = child.Key;
            object started = child.Child(LocalStorage.GAME_STARTED).Value;
            var roomInfoJson = child.Child(LocalStorage.ROOM_INFO).GetRawJsonValue();
            var roomInfo = JsonUtility.FromJson<RoomInfo>(roomInfoJson);
            if (roomInfoJson == null || roomInfo == null) {
                roomReference.Child(roomName).RemoveValueAsync();
                continue;
            }
            TimeSpan delta = DateTime.UtcNow.Subtract(Convert.ToDateTime(roomInfo.date));
            if (delta.Hours > 6 || delta.Days > 0) {
                roomReference.Child(roomName).RemoveValueAsync();
                continue;
            }
            var obj = rooms.Find(room => room.name == roomName);
            if (obj != null && roomInfo.keepPrivate || roomInfo.currentCount >= roomInfo.playerCount
                                                    || started == null || (bool) started
                                                    || delta.Days > 0
                                                    || delta.Hours > 6)
                RemoveRoom(obj);
            else if (obj == null) rooms.Add(CreateRoom(roomName, roomInfo));
            else obj.GetComponent<RoomUI>().UpdateData(roomInfo);
        }

        UpdateList();
    }

    private GameObject CreateRoom(string roomName, RoomInfo roomInfo) {
        GameObject obj = null;
        if (roomsPool.Count != 0) {
            obj = roomsPool.Pop();
            obj.SetActive(true);
            obj.transform.SetParent(content, false);
        } else obj = Instantiate(roomObject, content);

        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 30f);
        obj.GetComponent<RoomUI>().LoadData(roomName, roomInfo);
        ConnectToRoom connectToRoom = new ConnectToRoom(roomReference.Child(roomName), roomName, loadBoard);
        obj.GetComponent<RoomUI>().ClickObject.GetComponent<RoomClick>().Connect = connectToRoom;
        return obj;
    }
    private void RemoveRoom(GameObject obj) {
        if (obj == null) return;
        rooms.Remove(obj);
        roomsPool.Push(obj);
        obj.SetActive(false);
    }
    private void UpdateList() {
        for (int i = 0; i < rooms.Count; i++) {
            rooms[i].GetComponent<RoomUI>().UpdateColor(colors[i % 2]);
        }
    }
}
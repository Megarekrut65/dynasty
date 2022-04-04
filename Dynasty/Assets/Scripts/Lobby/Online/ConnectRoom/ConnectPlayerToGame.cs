using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class ConnectPlayerToGame {
      public static void Connect(DatabaseReference roomReference, string playerName, Action<Task> end) {
            roomReference.Child("players").GetValueAsync().ContinueWithOnMainThread(task => {
                  if (task.Exception != null) {
                        Debug.LogError(task.Exception);
                        return;
                  }
                  var playerKey = "1";
                  if (task.Result != null) {
                        List<DataSnapshot> list = new List<DataSnapshot>(task.Result.Children);
                        for(int i = 0; i < 6; i++) {
                              playerKey = (i + 1).ToString();
                              var key = playerKey;
                              var dataSnapshot = list.Find(data => data.Key == key);
                              if(dataSnapshot == null) break;
                        }
                  }
                  PlayerPrefs.SetString(PrefabsKeys.PLAYER_KEY, playerKey);
                  roomReference.Child("players").Child(playerKey).SetValueAsync(playerName).ContinueWithOnMainThread(end);
            });
      }
}
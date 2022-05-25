using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class PrintAboutPlayerInDatabase {
    public static void Print(DatabaseReference roomReference, string playerName, Action<Task> end) {
        roomReference.Child(GameKeys.PLAYERS).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                Debug.LogError(task.Exception);
                return;
            }

            var playerKey = "1";
            if (task.Result != null) {
                List<DataSnapshot> list = new List<DataSnapshot>(task.Result.Children);
                for (int i = 0; i < 6; i++) {
                    playerKey = (i + 1).ToString();
                    var key = playerKey;
                    var dataSnapshot = list.Find(data => data.Key == key);
                    if (dataSnapshot == null) break;
                }
            }

            PlayerPrefs.SetString(LocalStorage.PLAYER_KEY, playerKey);
            roomReference.Child("players").Child(playerKey).Child(LocalStorage.PLAYER_NAME)
                .SetValueAsync(playerName).ContinueWithOnMainThread(end);
        });
    }
}
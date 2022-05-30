using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class PlayerDatabaseUtilities {
    public static void PrintPlayerToDatabase(DatabaseReference roomReference, string playerName, Action<Task> end) {
        roomReference.Child(GameKeys.PLAYERS).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                Debug.LogError(task.Exception);
                return;
            }

            var playerKey = "1";
            if (task.Result != null) {
                List<DataSnapshot> list = new List<DataSnapshot>(task.Result.Children);
                playerKey = GetPlayerKey(key => {
                    return list.Find(data => data.Key == key);
                });
            }
            
            PlayerPrefs.SetString(LocalStorage.PLAYER_KEY, playerKey);
            roomReference.Child(GameKeys.PLAYERS).Child(playerKey).Child(LocalStorage.PLAYER_NAME)
                .SetValueAsync(playerName).ContinueWithOnMainThread(end);
        });
    }
    public static string GetPlayerKey(Func<string, object> check) {
        var playerKey = "1";
        for (int i = 0; i < 6; i++) {
            playerKey = (i + 1).ToString();
            var key = playerKey;
            var data = check(key);
            if (data == null) break;
        }

        return playerKey;
    }
}


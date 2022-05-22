using System;
using Firebase.Auth;
using UnityEngine;

public class LoadUser : MonoBehaviour {
    private void Start() {
        PlayerPrefs.SetString(LocalStorage.PLAYER_NAME, FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
    }
}
using System;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountManager:MonoBehaviour {
    [SerializeField]
    private Text nickname;

    public void SignOut() {
        SignInController.SignOutUser();
        SceneManager.LoadScene("SignIn", LoadSceneMode.Single);
    }
    private void Start() {
        nickname.text = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
    }
}
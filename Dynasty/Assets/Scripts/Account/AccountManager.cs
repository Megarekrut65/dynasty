using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages user account in Firebase Auth
/// </summary>
public class AccountManager : MonoBehaviour {
    /// <summary>Text object for nickname</summary>
    [SerializeField]
    private Text nickname;
    
    /// <summary>
    /// Signs out from account and open sign in scene
    /// </summary>
    public void SignOut() {
        SignInController.SignOutUser();
        SceneManager.LoadScene("SignIn", LoadSceneMode.Single);
    }
    /// <summary>
    /// Takes current user nickname and sets it to text object in scene
    /// </summary>
    private void Start() {
        nickname.text = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
    }
}
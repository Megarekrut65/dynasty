using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

/// <summary>
/// Checks current version of application and correct version in database
/// </summary>
public class VersionLoad : MonoBehaviour {
    public bool Ready { get; private set; }
    public bool CorrectVersion { get; private set; }
    private bool isStarted = false;
    
    public void StartChecking() {
        if(isStarted) return;
        isStarted = true;
        FirebaseDatabase.DefaultInstance.RootReference
            .Child("system").Child("gameVersion").GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.Exception != null) {
                    Debug.Log(task.Exception);
                    return;
                }
                CorrectVersion = (string) task.Result.Value == Application.version;
                Ready = true;
            });
    }
}
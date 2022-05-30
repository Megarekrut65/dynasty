using Firebase;
using Firebase.Extensions;
using UnityEngine;

/// <summary>
/// Loads database dependencies
/// </summary>
public class FirebaseLoad : MonoBehaviour {
    public bool Ready { get; private set; } = false;

    private void Start() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                Debug.LogError(task.Exception);
                return;
            }

            Ready = true;
        });
    }
}
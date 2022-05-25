using Firebase;
using Firebase.Extensions;
using UnityEngine;

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
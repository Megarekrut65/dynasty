using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseLoad:MonoBehaviour {
    public bool Ready { get; private set; } = false;
    
    private void Start(){
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                Debug.LogError(task.Exception);
                return;
            }
    
            Ready = true;
        });
    }
}
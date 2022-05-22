using System.Collections;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public static class SignUpController {
    public delegate void ErrorHandling(string message);
    public static event ErrorHandling Error;
    public delegate void SuccessfulHandling();
    public static event SuccessfulHandling Successful;

    public static void RegisterUser(string nickname, string email, string password) {
        var auth = FirebaseAuth.DefaultInstance;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Error?.Invoke("Registration was canceled due to some errors");
                return;
            }
            if (task.IsFaulted) {
                Error?.Invoke(Translator.Translate("already-exists"));
                return;
            }
            FirebaseUser newUser = task.Result;
            newUser.UpdateUserProfileAsync(new UserProfile{DisplayName = nickname}).ContinueWithOnMainThread(t => {
                if (t.Exception != null) {
                    Error?.Invoke("Registration was Faulted due to some errors: " + task.Exception);
                    return;
                }
                Successful?.Invoke();
            });
        });
    }
}

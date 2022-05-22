using Firebase.Auth;
using UnityEngine;

public static class SignInController {
    public delegate void ErrorHandling(string message);
    public static event ErrorHandling Error;
    public delegate void SuccessfulHandling();
    public static event SuccessfulHandling Successful;

    public static void SignInUser(string email, string password) {
        var auth = FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Error?.Invoke("Logging was canceled due to some errors");
                return;
            }
            if (task.IsFaulted) {
                Error?.Invoke("Logging was Faulted due to some errors: " + task.Exception);
                return;
            }
            Successful?.Invoke();
        });
    }
    public static bool IsUserSignIn() {
        return FirebaseAuth.DefaultInstance.CurrentUser != null;
    }
    public static void SignOutUser() {
        FirebaseAuth.DefaultInstance.SignOut();
    }
    public static void ResetPassword(string email) {
        var auth = FirebaseAuth.DefaultInstance;
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
            if (task.Exception != null) {
                Error?.Invoke("Password resetting has some errors: " + task.Exception);
                return;
            }
            Successful?.Invoke();
        });
    }
}
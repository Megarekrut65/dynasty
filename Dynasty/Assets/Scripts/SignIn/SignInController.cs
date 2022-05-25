using Firebase.Auth;
using Firebase.Extensions;

public static class SignInController {
    public delegate void ErrorSignInHandling(string message);
    public static event ErrorSignInHandling Error;
    public delegate void SuccessfulSignInHandling();
    public static event SuccessfulSignInHandling Successful;

    public static void SignInUser(string email, string password) {
        var auth = FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Error?.Invoke("Logging was canceled due to some errors");
                return;
            }

            if (task.IsFaulted) {
                Error?.Invoke(Translator.Translate("doesn't-exist"));
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
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                Error?.Invoke(Translator.Translate("invalid-email"));
                return;
            }

            Successful?.Invoke();
        });
    }
}
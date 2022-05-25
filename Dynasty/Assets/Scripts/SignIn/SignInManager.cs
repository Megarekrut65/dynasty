using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInManager : MonoBehaviour {
    [Header("Boards")]
    [SerializeField]
    private GameObject blackBoard;
    [SerializeField]
    private GameObject redBoard;
    [SerializeField]
    private GameObject toast;
    [SerializeField]
    private GameObject canvas;
    private LoadBoard loadBoard;
    private ErrorBoard errorBoard;
    private ToastBoard toastBoard;
    [Header("Form parts")]
    [SerializeField]
    private InputField email;
    [SerializeField]
    private InputField password;

    private void Start() {
        loadBoard = new LoadBoard(blackBoard, canvas);
        errorBoard = new ErrorBoard(redBoard, canvas);
        toastBoard = new ToastBoard(toast, canvas);
        SignInController.Successful += Logged;
        SignInController.Error += ErrorHandle;
    }
    private void OnDestroy() {
        SignInController.Successful -= Logged;
        SignInController.Error -= ErrorHandle;
    }
    public void SignIn() {
        loadBoard.SetActive(true);
        SignInController.SignInUser(email.text, password.text);
    }
    public void ForgotPassword() {
        if (email.text.Length >= 6) {
            loadBoard.SetActive(true);
            toastBoard.ShowMessage(Translator.Translate("send-to-email"), 5.0f);
            SignInController.ResetPassword(email.text);
            return;
        }

        errorBoard.SetActive(true);
        errorBoard.SetMessage(Translator.Translate("email is too short"));
    }
    private void Logged() {
        loadBoard.SetActive(false);
        if (SignInController.IsUserSignIn()) SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    private void ErrorHandle(string error) {
        loadBoard.SetActive(false);
        errorBoard.SetActive(true);
        errorBoard.SetMessage(Translator.Translate(error));
    }
}
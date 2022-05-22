using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpManager:MonoBehaviour {
    [Header("Boards")]
    [SerializeField]
    private GameObject blackBoard;
    [SerializeField]
    private GameObject redBoard;
    [SerializeField]
    private GameObject canvas;
    private LoadBoard loadBoard;
    private ErrorBoard errorBoard;
    [Header("Form parts")]
    [SerializeField]
    private InputField nickname;
    [SerializeField]
    private InputField email;
    [SerializeField]
    private InputField password;
    [SerializeField]
    private InputField confirmPassword;
    private const int MIN_LENGTH = 6;
    private void Start() {
        loadBoard = new LoadBoard(blackBoard, canvas);
        errorBoard = new ErrorBoard(redBoard, canvas);
        SignUpController.Successful += Registered;
        SignUpController.Error += ErrorHandle;
    }
    private void OnDestroy() {
        SignUpController.Successful -= Registered;
        SignUpController.Error -= ErrorHandle;
    }
    public void RegisterUser() {
        if (CheckForLength("nickname", nickname)
            && CheckForLength("email", email)
            && CheckForLength("password", password)
            && CheckForLength("confirm-password", confirmPassword)
            && CheckPasswords()) {
            loadBoard.SetActive(true);
            SignUpController.RegisterUser(nickname.text, email.text, password.text);
        }
    }
    private void Registered() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    private void ErrorHandle(string error) {
        loadBoard.SetActive(false);
        errorBoard.SetActive(true);
        errorBoard.SetMessage(Translator.Translate(error));
    }
    private bool CheckForLength(string fieldName, InputField field) {
        if (field.text.Length < MIN_LENGTH) {
            errorBoard.SetActive(true);
            errorBoard.SetMessage(Translator.Translate(fieldName + " is too short"));
            return false;
        }

        return true;
    }
    private bool CheckPasswords() {
        if (password.text != confirmPassword.text) {
            errorBoard.SetActive(true);
            errorBoard.SetMessage(Translator.Translate("passwords-don't-match"));
            return false;
        }

        return true;
    }
}
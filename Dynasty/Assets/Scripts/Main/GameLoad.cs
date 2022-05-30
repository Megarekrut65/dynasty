using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Loads all game dependencies
/// </summary>
public class GameLoad : MonoBehaviour {
    [SerializeField]
    private Slider loadSlider;
    [SerializeField]
    private FirebaseLoad firebaseLoad;
    [SerializeField]
    private VersionLoad versionLoad;

    private void Start() {
        LocalStorage.GetValue(LocalStorage.PLAYER_NAME, "Player" + Random.Range(0, 100));
        loadSlider.value = 0;
        StartCoroutine(LoadData());
    }
    private IEnumerator LoadData() {
        while (Math.Abs(loadSlider.value - 100) > 0.000f) {
            yield return new WaitForSeconds(0.01f);
            if(firebaseLoad.Ready) versionLoad.StartChecking();
            if (LocalizationManager.Instance.Ready && firebaseLoad.Ready && versionLoad.Ready)
                loadSlider.value += 10;
            else if (loadSlider.value < 90) loadSlider.value++;
        }

        SceneManager.LoadScene(versionLoad.CorrectVersion
            ? SignInController.IsUserSignIn() ? "Menu" : "SignIn"
            : "Update", LoadSceneMode.Single);
    }
}
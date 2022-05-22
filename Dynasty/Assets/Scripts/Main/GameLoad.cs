using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameLoad : MonoBehaviour {
	[SerializeField]
	private Slider loadSlider;
	[SerializeField]
	private FirebaseLoad firebaseLoad;
	
	private void Start() {
		LocalStorage.GetValue(LocalStorage.PLAYER_NAME, "Player" + Random.Range(0, 100));
		loadSlider.value = 0;
		StartCoroutine(LoadData());
	}
	private IEnumerator LoadData() {
		while (Math.Abs(loadSlider.value - 100) > 0.000f) {
			yield return new WaitForSeconds(0.01f);
			if (LocalizationManager.Instance.Ready && firebaseLoad.Ready)
				loadSlider.value += 10;
			else if (loadSlider.value < 90) loadSlider.value++;
		}
		if(SignInController.IsUserSignIn())
			SceneManager.LoadScene("Menu", LoadSceneMode.Single);
		else SceneManager.LoadScene("SignIn", LoadSceneMode.Single);
	}
}

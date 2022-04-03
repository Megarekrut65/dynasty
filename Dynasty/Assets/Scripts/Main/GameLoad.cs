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
	private string sceneName;
	[SerializeField]
	private FirebaseLoad firebaseLoad;
	
	private void Start() {
		PrefabsKeys.GetValue(PrefabsKeys.PLAYER_NAME, "Player" + Random.Range(0, 100));
		loadSlider.value = 0;
		StartCoroutine(LoadData());
	}
	private IEnumerator LoadData() {
		while (Math.Abs(loadSlider.value - 100) > 0.000f) {
			yield return new WaitForSeconds(0.005f);
			if (LocalizationManager.Instance.Ready && firebaseLoad.Ready)
				loadSlider.value += 10;
			else if (loadSlider.value < 90) loadSlider.value++;
		}

		SceneManager.LoadScene(this.sceneName, LoadSceneMode.Single);
	}
}

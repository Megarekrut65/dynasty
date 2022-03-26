using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour {
	[SerializeField]
	private Slider loadSlider;
	[SerializeField]
	private string sceneName;

	private void Start() {
		loadSlider.value = 0;
		StartCoroutine(LoadData());
	}
	private IEnumerator LoadData() {
		while (loadSlider.value != 100) {
			yield return new WaitForSeconds(0.005f);
			if (LocalizationManager.Instance.Ready)
				loadSlider.value += 10;
			else if (loadSlider.value < 90) loadSlider.value++;
		}

		SceneManager.LoadScene(this.sceneName, LoadSceneMode.Single);
	}
}

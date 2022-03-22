using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CoverCard : MonoBehaviour {
	[SerializeField]
	private GameObject coverImage;
	[SerializeField]
	public GameObject coverContainer;
	public void Cover() {
		coverImage.SetActive(true);
	}
	public void Uncover() {
		coverImage.SetActive(false);
	}
	public void AddToContainer(GameObject obj) {
		obj.transform.SetParent(coverContainer.transform, false);
		StartCoroutine(Alignment());
	}
	private IEnumerator Alignment() {
		yield return new WaitForSeconds(0.1f);
		var group = coverContainer.
			GetComponent<HorizontalLayoutGroup>();
		group.childAlignment = TextAnchor.MiddleCenter;
	}
}
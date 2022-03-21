using UnityEngine;

public class CoverCard : MonoBehaviour {
	[SerializeField]
	private GameObject coverImage;

	public void Cover() {
		coverImage.SetActive(true);
	}
	public void Uncover() {
		coverImage.SetActive(false);
	}
}